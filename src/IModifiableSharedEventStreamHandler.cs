namespace OwlCore.ComponentModel.Nomad;

/// <summary>
/// A shared event stream handler backed by content on Ipfs.
/// </summary>
/// <typeparam name="TEventEntryUpdate">The type stored within each event stream's <see cref="EventStreamEntry{TContentPointer}.Content"/>.</typeparam>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStreamEntry">A type containing the metadata for an event stream entry.</typeparam>
/// <typeparam name="TEventStreamSource">The type used to specify multiple event stream sources during replay.</typeparam>
/// <typeparam name="TListeningHandlers">The type of listener/handler instances for this event stream.</typeparam>
public interface IModifiableSharedEventStreamHandler<in TEventEntryUpdate, TContentPointer, TEventStreamSource, TEventStreamEntry, TListeningHandlers> : IModifiableEventStreamHandler<TEventEntryUpdate, TContentPointer, TEventStreamEntry>, ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry, TListeningHandlers>
    where TEventStreamSource : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
    where TListeningHandlers : ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry, TListeningHandlers>
{
}

/// <summary>
/// A shared event stream handler backed by content on Ipfs.
/// </summary>
/// <typeparam name="TEventEntryContent">The type stored within each event stream's <see cref="EventStreamEntry{TContentPointer}.Content"/>.</typeparam>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
/// <typeparam name="TEventStreamEntry">A type containing the metadata for an event stream entry.</typeparam>
/// <typeparam name="TEventStreamSource">The type used to specify multiple event stream sources during replay.</typeparam>
public interface IModifiableSharedEventStreamHandler<in TEventEntryContent, TContentPointer, TEventStreamSource, TEventStreamEntry> : IModifiableEventStreamHandler<TEventEntryContent, TContentPointer, TEventStreamEntry>, ISharedEventStreamHandler<TContentPointer, TEventStreamSource, TEventStreamEntry>
    where TEventStreamSource : EventStream<TContentPointer>
    where TEventStreamEntry : EventStreamEntry<TContentPointer>
{
}