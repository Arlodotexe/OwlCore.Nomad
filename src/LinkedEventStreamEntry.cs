using System;

namespace OwlCore.ComponentModel.Nomad;

/// <summary>
/// Represents the data for a single entry in an event stream that can be linked back to a prior entry.
/// </summary>
/// <typeparam name="TContentPointer">An immutable pointer to data in the event stream.</typeparam>
public record LinkedEventStreamEntry<TContentPointer> : EventStreamEntry<TContentPointer>
{
    /// <summary>
    /// Represents the stream entry prior to this one, if any.
    /// </summary>
    public TContentPointer? PriorEventStreamEntry { get; init; }
}
