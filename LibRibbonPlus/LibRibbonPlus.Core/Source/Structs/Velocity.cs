// <copyright file="Velocity.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents something that travels at a specific speed.
/// </summary>
/// <param name="Amount">The speed.</param>
public readonly record struct Velocity(float Amount)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Velocity Blank { get; } = new(1);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{Amount.ToString(CultureInfo.InvariantCulture)}x";

    /// <summary>
    /// Tries to create a new instance of <see cref="Velocity"/> from a given <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to parse.</param>
    /// <param name="velocity">The result, or <see cref="Blank"/>.</param>
    /// <returns>The value <see langword="true"/> if the parse is successful, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(Chars chars, out Velocity velocity)
    {
        velocity = Blank;

        if (chars[^1] is 'x')
            chars = chars[..^1];

        if (!float.TryParse(chars.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float amount))
            return false;

        velocity = new(amount);

        return true;
    }
}
