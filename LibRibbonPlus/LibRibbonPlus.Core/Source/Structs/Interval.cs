// <copyright file="Interval.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Encapsulates a number that works similarily to <see cref="TimeSpan"/> which specifies an occurence at a specific time.
/// </summary>
/// <param name="Microseconds">The amount of microseconds elapsed before something happens.</param>
public readonly record struct Interval(long Microseconds)
{
    /// <summary>
    /// Gets the amount of microseconds the main timing window of the game is.
    /// </summary>
    public static Interval Leniency { get; } = new(50_000);

    /// <summary>
    /// Gets the amount of microseconds the main rendering window of the game is.
    /// </summary>
    public static Interval Rendering { get; } = new(60_000_000);

    private const double MicrosecondsInASecond = 1_000_000D;

    private const string MicrosecondSuffix = "μs";

    /// <summary>
    /// Converts an <see cref="Interval"/> to a <see cref="float"/> represented in microseconds.
    /// </summary>
    /// <param name="interval">The amount of time.</param>
    public static explicit operator float(Interval interval) => (float)(interval.Microseconds / MicrosecondsInASecond);

    /// <summary>
    /// Converts a <see cref="float"/> represented in seconds to an instance of <see cref="Interval"/>.
    /// </summary>
    /// <param name="seconds">The amount of seconds.</param>
    public static explicit operator Interval(float seconds) => new((long)(seconds * MicrosecondsInASecond));

    /// <summary>
    /// Compares two <see cref="Interval"/> values to see if the left-hand side is larger than the right.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to compare.</param>
    /// <param name="right">The second <see cref="Interval"/> to compare.</param>
    /// <returns>The value <see langword="true"/> if <paramref name="left"/> is larger than <paramref name="right"/>, otherwise <see langword="false"/>.</returns>
    public static bool operator >(Interval left, Interval right) => left.Microseconds > right.Microseconds;

    /// <summary>
    /// Compares two <see cref="Interval"/> values to see if the left-hand side is smaller than the right.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to compare.</param>
    /// <param name="right">The second <see cref="Interval"/> to compare.</param>
    /// <returns>The value <see langword="true"/> if <paramref name="left"/> is larger than <paramref name="right"/>, otherwise <see langword="false"/>.</returns>
    public static bool operator <(Interval left, Interval right) => left.Microseconds < right.Microseconds;

    /// <summary>
    /// Compares two <see cref="Interval"/> values to see if the left-hand side is larger or equal the right.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to compare.</param>
    /// <param name="right">The second <see cref="Interval"/> to compare.</param>
    /// <returns>The value <see langword="true"/> if <paramref name="left"/> is larger than <paramref name="right"/>, otherwise <see langword="false"/>.</returns>
    public static bool operator >=(Interval left, Interval right) => left.Microseconds >= right.Microseconds;

    /// <summary>
    /// Compares two <see cref="Interval"/> values to see if the left-hand side is smaller or equal the right.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to compare.</param>
    /// <param name="right">The second <see cref="Interval"/> to compare.</param>
    /// <returns>The value <see langword="true"/> if <paramref name="left"/> is larger than <paramref name="right"/>, otherwise <see langword="false"/>.</returns>
    public static bool operator <=(Interval left, Interval right) => left.Microseconds <= right.Microseconds;

    /// <summary>
    /// Gets the sum of two <see cref="Interval"/> values.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to add.</param>
    /// <param name="right">The second <see cref="Interval"/> to add.</param>
    /// <returns>An <see cref="Interval"/> representing the sum of both elements.</returns>
    public static Interval operator +(Interval left, Interval right) => new(left.Microseconds + right.Microseconds);

    /// <summary>
    /// Gets the difference of two <see cref="Interval"/> values.
    /// </summary>
    /// <param name="left">The first <see cref="Interval"/> to subtract.</param>
    /// <param name="right">The second <see cref="Interval"/> to subtract.</param>
    /// <returns>An <see cref="Interval"/> representing the difference of both elements.</returns>
    public static Interval operator -(Interval left, Interval right) => new(left.Microseconds - right.Microseconds);

    /// <summary>
    /// Tries to create a new instance of <see cref="Interval"/> from a given <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to parse.</param>
    /// <param name="interval">The result, or 0.</param>
    /// <returns>The value <see langword="true"/> if the parse is successful, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(Chars chars, out Interval interval)
    {
        if (chars[..^2] == MicrosecondSuffix)
            chars = chars[^2..];

        bool output = long.TryParse(chars.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out long result);

        interval = new(result);

        return output;
    }

    /// <inheritdoc/>
    public override readonly string ToString() => $"{Microseconds.ToStringInvariant()}{MicrosecondSuffix}";
}
