// <copyright file="Note.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Encapsulates data for a single note.
/// </summary>
/// <param name="Kind">The types of obstacles this note contains.</param>
/// <param name="Velocity">The speed in which the obstacle flies.</param>
/// <param name="Interval">The specific point in which the obstacle is expected to be reached.</param>
public record Note(Obstacles Kind, Velocity Velocity, Interval Interval) : ITimed
{
    /// <summary>
    /// Contains the separator between time and speed.
    /// </summary>
    public const string At = " at ";

    /// <summary>
    /// Contains the separator for a prefix and the raw interval.
    /// </summary>
    public const string Separator = ": ";

    /// <summary>
    /// Gets or sets a value indicating how much increased shake should be added to this note.
    /// </summary>
    public float IncreasedShake { get; set; } = 0;

    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Note Blank { get; } = new(default, Velocity.Blank, default);

    /// <summary>
    /// Tries to create a new instance of <see cref="Note"/> from a given <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to parse.</param>
    /// <param name="note">The result, or <see cref="Blank"/>.</param>
    /// <returns>The value <see langword="true"/> if the parse is successful, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(Chars chars, out Note note)
    {
        note = Blank;

        if (!chars.TrySplit(Separator, out Chars before, out Chars obstacles))
            return false;

        if (!before.TrySplit(At, out Chars time, out Chars speed))
            return false;

        if (!Enum.TryParse(obstacles.ToString(), out Obstacles type))
            return false;

        if (!Velocity.TryParse(speed, out Velocity velocity))
            return false;

        if (!Interval.TryParse(time, out Interval interval))
            return false;

        note = new(type, velocity, interval);

        return true;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Interval}{At}{Velocity}: {Kind}";
}
