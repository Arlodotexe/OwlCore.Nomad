﻿using OwlCore.ComponentModel;
using OwlCore.ComponentModel.Nomad;
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
    public static async IAsyncEnumerable<TEventStreamEntry> AdvanceFullEventStreamAsync<TContentPointer, TEventStreamSource, TEventStreamEntry>(ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry> eventStreamHandler, Func<TContentPointer, CancellationToken, Task<TEventStreamEntry>> contentPointerToStreamEntryAsync, [EnumeratorCancellation] CancellationToken cancellationToken)
        where TEventStreamSource : EventStream<TContentPointer>
        where TEventStreamEntry : EventStreamEntry<TContentPointer>
    {
        // Resolve all events in stream
        var resolvedEventStreamEntries = await eventStreamHandler.Sources.ResolveEventStreamsAsync(contentPointerToStreamEntryAsync, cancellationToken);

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
    /// Resolves the full event stream from all sources, organized by date.
    /// </summary>
    /// <param name="contentPointerToStreamEntryAsync">A method to convert a <typeparamref name="TContentPointer"/> to a <typeparamref name="TEventStreamEntry"/>.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <param name="eventStreams">The event stream sources to resolve.</param>
    public static async Task<IEnumerable<TEventStreamEntry>> ResolveEventStreamsAsync<TContentPointer, TEventStreamSource, TEventStreamEntry>(this IEnumerable<TEventStreamSource> eventStreams, Func<TContentPointer, CancellationToken, Task<TEventStreamEntry>> contentPointerToStreamEntryAsync, CancellationToken cancellationToken)
        where TEventStreamSource : EventStream<TContentPointer>
        where TEventStreamEntry : EventStreamEntry<TContentPointer>
    {
        // Get all event entries across all sources
        var allEventEntries = await eventStreams
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