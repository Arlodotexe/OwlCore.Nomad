using CommunityToolkit.Diagnostics;
using OwlCore.ComponentModel;
using OwlCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.Nomad.Extensions;

/// <summary>
/// Extension methods for <see cref="EventStream{TEventEntryContent}"/>s and <see cref="IEventStreamHandler{TEventStreamEntry}"/>s.
/// </summary>
public static class EventStreamExtensions
{
    /// <summary>
    /// Resolves the full event stream from all sources organized by date, advancing all listening <see cref="ISharedEventStreamHandler{TContentPointer,TEventStreamSource,TEventStreamEntry,TListeningHandlers}.ListeningEventStreamHandlers"/> on the given <paramref name="eventStreamHandler"/> using data from all available <see cref="ISources{TEventStreamSource}.Sources"/>.
    /// </summary>
    public static async IAsyncEnumerable<TEventStreamEntry> AdvanceSharedEventStreamAsync<TContentPointer, TEventStreamSource, TEventStreamEntry>(this ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry> eventStreamHandler, Func<TContentPointer, CancellationToken, Task<TEventStreamSource>> contentPointerToEventStreamSourceAsync, Func<TContentPointer, CancellationToken, Task<TEventStreamEntry>> contentPointerToStreamEntryAsync, [EnumeratorCancellation] CancellationToken cancellationToken)
        where TEventStreamSource : EventStream<TContentPointer>
        where TEventStreamEntry : EventStreamEntry<TContentPointer>
    {
        // Resolve all events in stream
        var resolvedEventStreamEntries = await eventStreamHandler.Sources.ResolveEventStreamEntriesAsync(contentPointerToEventStreamSourceAsync, contentPointerToStreamEntryAsync, cancellationToken);

        // Playback event stream
        // Order event entries by oldest first
        foreach (var eventEntry in resolvedEventStreamEntries.OrderBy(x => x.TimestampUtc))
        {
            // Advance event stream for all listening objects
            await eventStreamHandler.ListeningEventStreamHandlers
                .Where(x => x.Id == eventEntry.Id)
                .InParallel(x => x.TryAdvanceEventStreamAsync(eventEntry, cancellationToken));

            yield return eventEntry;
        }
    }

    /// <summary>
    /// Resolves the full event stream from all <see cref="ISources{T}.Sources"/> organized by date and advances the <paramref name="eventStreamHandler"/> to the given <paramref name="maxDateTimeUtc"/>.
    /// </summary>
    public static async IAsyncEnumerable<TEventStreamEntry> AdvanceEventStreamToAtLeastAsync<TContentPointer, TEventStreamSource, TEventStreamEntry>(this ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry> eventStreamHandler, DateTime maxDateTimeUtc, Func<TContentPointer, CancellationToken, Task<TEventStreamSource>> contentPointerToEventStreamSourceAsync, Func<TContentPointer, CancellationToken, Task<TEventStreamEntry>> contentPointerToStreamEntryAsync, [EnumeratorCancellation] CancellationToken cancellationToken)
        where TEventStreamSource : EventStream<TContentPointer>
        where TEventStreamEntry : EventStreamEntry<TContentPointer>
    {
        // Resolve all events in stream
        var resolvedEventStreamEntries = await eventStreamHandler.Sources.ResolveEventStreamEntriesAsync(contentPointerToEventStreamSourceAsync, contentPointerToStreamEntryAsync, cancellationToken);

        // Playback event stream
        // Order event entries by oldest first
        foreach (var eventEntry in resolvedEventStreamEntries
                     .OrderBy(x => x.TimestampUtc)
                     .Where(x => (x.TimestampUtc ?? ThrowHelper.ThrowArgumentNullException<DateTime>()) <= maxDateTimeUtc))
        {
            // Advance event stream for all listening objects
            await eventStreamHandler.TryAdvanceEventStreamAsync(eventEntry, cancellationToken);
            yield return eventEntry;
        }
    }

    /// <summary>
    /// Resolves the full event stream from all sources, organized by date.
    /// </summary>
    /// <param name="contentPointerToEventStreamSourceAsync">A method to convert a <typeparamref name="TContentPointer"/> to a <typeparamref name="TEventStreamSource"/>.</param>
    /// <param name="contentPointerToStreamEntryAsync">A method to convert a <typeparamref name="TContentPointer"/> to a <typeparamref name="TEventStreamEntry"/>.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <param name="sources">The event stream sources to resolve.</param>
    public static async Task<IEnumerable<TEventStreamEntry>> ResolveEventStreamEntriesAsync<TContentPointer, TEventStreamSource, TEventStreamEntry>(this IEnumerable<TContentPointer> sources, Func<TContentPointer, CancellationToken, Task<TEventStreamSource>> contentPointerToEventStreamSourceAsync, Func<TContentPointer, CancellationToken, Task<TEventStreamEntry>> contentPointerToStreamEntryAsync, CancellationToken cancellationToken)
        where TEventStreamSource : EventStream<TContentPointer>
        where TEventStreamEntry : EventStreamEntry<TContentPointer>
    {
        var allEventStreamSources = await sources
            .InParallel(x => contentPointerToEventStreamSourceAsync(x, cancellationToken));

        // Get all event entries across all sources
        var allEventEntries = await allEventStreamSources
            .Select(x => x.Entries)
            .Aggregate((x, y) =>
            {
                foreach (var item in y)
                    x.Add(item);
                return x;
            })
            .InParallel(x => contentPointerToStreamEntryAsync(x, cancellationToken));

        var sortedEventEntries = allEventEntries
            .OrderBy(x => x.TimestampUtc);

        return sortedEventEntries;
    }
}