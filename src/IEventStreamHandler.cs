using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OwlCore.ComponentModel;

namespace OwlCore.Nomad;

/// <summary>
/// Represents an object that can supports seeking via a generic data type.
/// </summary>
/// <typeparam name="TEventStream">The type used for the local event stream.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
public interface IEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry> : ISources<TContentPointer>
    where TEventStream : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
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
    /// A collection of all event stream entries resolved from the <see cref="ISources{TContentPointer}.Sources"/>.
    /// </summary>
    /// <remarks>Must contain entries that target this <see cref="EventStreamHandlerId"/>, may contain entries that don't target this instance by some application-defined criteria.</remarks>
    public ICollection<TEventStreamEntry> AllEventStreamEntries { get; set; }
    
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
