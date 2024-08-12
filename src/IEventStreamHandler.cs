using System.Threading;
using System.Threading.Tasks;

namespace OwlCore.Nomad;

/// <summary>
/// Represents an object that can supports seeking via a generic data type.
/// </summary>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
public interface IEventStreamHandler<TEventStreamEntry>
{
    /// <summary>
    /// A unique identifier for this event stream handler.
    /// </summary>
    public string EventStreamId { get; }

    /// <summary>
    /// The current position in the event stream and the furthest point that <see cref="AdvanceEventStreamAsync"/> has been successfully called.
    /// </summary>
    public TEventStreamEntry? EventStreamPosition { get; set; }
    
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
