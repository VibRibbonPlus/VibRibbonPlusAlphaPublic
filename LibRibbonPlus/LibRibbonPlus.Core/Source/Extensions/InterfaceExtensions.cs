// <copyright file="InterfaceExtensions.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Provides extension methods for interfaces.
/// </summary>
public static class InterfaceExtensions
{
    /// <summary>
    /// Converts an <see cref="IConvertible"/> to a <see cref="string"/> with <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    /// <param name="convertible">The value to get a <see cref="string"/> representation of.</param>
    /// <returns>A culture-invariant <see cref="string"/> representation of <paramref name="convertible"/>.</returns>
    public static string ToStringInvariant(this IConvertible convertible)
        => convertible.ToString(CultureInfo.InvariantCulture);
}
