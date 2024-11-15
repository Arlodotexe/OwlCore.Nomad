using System.Collections.Generic;

namespace OwlCore.Nomad;

/// <summary>
/// Represents a specific stream of Nomad events that can be serialized.
/// </summary>
public record EventStream<TEventEntryContent>
{
    /// <summary>
    /// Display label for general use.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// A collection of <typeparamref name="TEventEntryContent"/>s for this content.
    /// </summary>
    public ICollection<TEventEntryContent> Entries { get; init; } = [];
}
