// <copyright file="Label.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Encapsulates an item that is labelable.
/// </summary>
/// <param name="Name">The name.</param>
public sealed record Label(string Name)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Label Blank { get; } = new(string.Empty);

    /// <inheritdoc/>
    public override string ToString() => Name;
}
