using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.Nomad;

/// <summary>
/// Represents a storable item that can be updated via an event stream.
/// </summary>
/// <typeparam name="TEventEntryUpdate">The type stored within each event stream's <see cref="EventStreamEntry{TContentPointer}.Content"/>.</typeparam>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStreamEntry">A type containing the metadata for an event stream entry.</typeparam>
public interface IModifiableEventStreamHandler<in TEventEntryUpdate, in TContentPointer, TEventStreamEntry> : IEventStreamHandler<TEventStreamEntry>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
{
    /// <summary>
    /// Appends the provided event entry of type <typeparamref name="TEventEntryUpdate"/> to the underlying event stream.
    /// </summary>
    /// <param name="updateEvent">The update event data to apply and persist.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <returns>A task containing the event stream entry that was applied from the update.</returns>
    public Task<TEventStreamEntry> AppendNewEntryAsync(TEventEntryUpdate updateEvent, CancellationToken cancellationToken = default);
}