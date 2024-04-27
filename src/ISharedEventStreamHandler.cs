using System.Collections.Generic;

namespace OwlCore.ComponentModel.Nomad;

/// <summary>
/// Represents an object that supports seeking via a consolidated multi-source event stream.
/// </summary>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStreamSource">The type used to specify multiple event stream sources during replay.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
/// <typeparam name="TListeningHandlers">The type of listener/handler instances for this event stream.</typeparam>
public interface ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry, TListeningHandlers> : IEventStreamHandler<TEventStreamEntry>, ISources<TEventStreamSource>, IHasId
    where TEventStreamSource : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
    where TListeningHandlers : ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry, TListeningHandlers>
{
    /// <summary>
    /// A shared collection of all available event streams that should participate in playback of events using their respective <see cref="IEventStreamHandler{T}.TryAdvanceEventStreamAsync"/>. 
    /// </summary>
    /// <remarks>
    /// Add the current <see cref="ISharedEventStreamHandler{TEventEntryContent,TEventSources, TEventHandlerEntry, TListeningHandlers}"/> instance to your list. This list should be trickled-down to all instances of <see cref="ISharedEventStreamHandler{TEventEntryContent,TEventSources, TEventHandlerEntry, TListeningHandlers}"/> throughout the application that wishes to receive events.
    /// </remarks>
    public ICollection<TListeningHandlers> ListeningEventStreamHandlers { get; }
}

/// <summary>
/// Represents an object that supports seeking via a consolidated multi-source event stream.
/// </summary>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStreamSource">The type used to specify multiple event stream sources during replay.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
public interface ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry> : ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry, ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry>>
    where TEventStreamSource : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
{
}