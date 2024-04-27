using System.Collections.Generic;

namespace OwlCore.ComponentModel.Nomad;

/// <summary>
/// Represents a specific stream of Nomad events that can be serialized.
/// </summary>
public record EventStream<TEventEntryContent> : IHasId
{
    /// <summary>
    /// Uniquely identifies this distributed event stream. This ID should be the same on all nodes.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display label for general use.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// A list of <typeparamref name="TEventEntryContent"/>s for this content.
    /// </summary>
    public HashSet<TEventEntryContent> Entries { get; init; } = new();
}
