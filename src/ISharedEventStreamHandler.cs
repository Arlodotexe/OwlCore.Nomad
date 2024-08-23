using System.Collections.Generic;
using OwlCore.ComponentModel;

namespace OwlCore.Nomad;

/// <summary>
/// Represents an object that supports seeking via a consolidated multi-source event stream.
/// </summary>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStream">The type used to specify multiple event stream sources during replay.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
/// <typeparam name="TEventEntryUpdate">The type stored within each event stream's <see cref="EventStreamEntry{TContentPointer}.Content"/>.</typeparam>
/// <typeparam name="TListeningHandlers">The type of listener/handler instances for this event stream.</typeparam>
public interface ISharedEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry, TListeningHandlers> : IEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry>
    where TEventStream : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
    where TListeningHandlers : ISharedEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry, TListeningHandlers>
{
    /// <summary>
    /// A shared collection of all available event streams that should participate in playback of events using their respective <see cref="IEventStreamHandler{TContentPointer, TEventStream, TEventStreamEntry, TEventEntryUpdate}.AdvanceEventStreamAsync"/>. 
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
/// <typeparam name="TEventStream">The type used to specify multiple event stream sources during replay.</typeparam>
/// <typeparam name="TEventStreamEntry">The type for the resolved event stream entries.</typeparam>
/// <typeparam name="TEventEntryUpdate">The type stored within each event stream's <see cref="EventStreamEntry{TContentPointer}.Content"/>.</typeparam>
public interface ISharedEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry> : ISharedEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry, ISharedEventStreamHandler<TContentPointer, TEventStream, TEventStreamEntry>>
    where TEventStream : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
{
}