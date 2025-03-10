using System.Threading;
using System.Threading.Tasks;
using OwlCore.ComponentModel;

namespace OwlCore.Nomad;

/// <summary>
/// Represents an object that can supports seeking via a generic data type.
/// </summary>
/// <typeparam name="TEventStream">The type used for the local event stream.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
/// <typeparam name="TImmutablePointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TMutablePointer">A pointer to mutable data in the event stream.</typeparam>
public interface IEventStreamHandler<TImmutablePointer, TMutablePointer, TEventStream, TEventStreamEntry> : ISources<TMutablePointer>
    where TEventStream : EventStream<TImmutablePointer>
    where TEventStreamEntry : EventStreamEntry<TImmutablePointer>
{
    /// <summary>
    /// A unique identifier for this event stream handler.
    /// </summary>
    public string EventStreamHandlerId { get; }

    /// <summary>
    /// The current position in the event stream and the furthest point that <see cref="AdvanceEventStreamAsync"/> has been successfully called.
    /// </summary>
    public TEventStreamEntry? EventStreamPosition { get; set; }
    
    /// <summary>
    /// The local event stream for this handler.
    /// </summary>
    public TEventStream LocalEventStream { get; set; }
    
    /// <summary>
    /// Advance the object's event stream using the given event.
    /// </summary>
    /// <remarks>
    /// This should be called when a new event is received from a source, or to fast-forward an object state.
    /// <para/>
    /// 
    /// </remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task AdvanceEventStreamAsync(TEventStreamEntry streamEntry, CancellationToken cancellationToken);

    /// <summary>
    /// Resets the event stream to a clean slate.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the ongoing operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task ResetEventStreamPositionAsync(CancellationToken cancellationToken);
}
