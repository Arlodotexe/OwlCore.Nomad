using System;
namespace OwlCore.Nomad;

/// <summary>
/// Represents the data for a single entry in an event stream.
/// </summary>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
public record EventStreamEntry<TContentPointer>
{
    /// <summary>
    /// A unique identifier corresponding to an object this event was applied to. This should be unique identical across runs and devices.
    /// </summary>
    public required string TargetId { get; init; }
    
    /// <summary>
    /// A unique identifier for the event that was applied.
    /// </summary>
    public required string EventId { get; init; }
    
    /// <summary>
    /// The UTC DateTime that the event occurred.
    /// </summary>
    public required DateTime? TimestampUtc { get; init; }

    /// <summary>
    /// A pointer to the content for this event entry that describes the change.
    /// </summary>
    public required TContentPointer Content { get; init; }
}
