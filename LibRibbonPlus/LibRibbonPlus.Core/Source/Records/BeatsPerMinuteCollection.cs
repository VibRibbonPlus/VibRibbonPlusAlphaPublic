// <copyright file="BeatsPerMinuteCollection.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Encapsulates an <see cref="Array"/> of <see cref="BeatsPerMinute"/>.
/// </summary>
/// <param name="Collection">The collection to encapsulate.</param>
public sealed record BeatsPerMinuteCollection(BeatsPerMinute[] Collection)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static BeatsPerMinuteCollection Blank { get; } = new(Array.Empty<BeatsPerMinute>());

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder builder = new();

        Collection.ForEach(beats => builder.Append(beats).AppendLine());

        return builder.ToString();
    }
}
