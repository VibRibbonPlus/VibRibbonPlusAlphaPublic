// <copyright file="FormatTryer.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Allows for shorthand in <see cref="ChartBuilder"/>.
/// </summary>
/// <param name="Prefix">The prefix to identify the action with.</param>
/// <param name="Apply">The action to run when the <paramref name="Prefix"/> is applied.</param>
public sealed record FormatTryer(string Prefix, Action<string> Apply)
{
    /// <summary>
    /// Constructs a <see cref="FormatTryer"/> from its tuple representation.
    /// </summary>
    /// <param name="tuple">The tuple to construct <see cref="FormatTryer"/> from.</param>
    public static implicit operator FormatTryer((string Prefix, Action<string> Apply) tuple) => new(tuple.Prefix, tuple.Apply);
}
