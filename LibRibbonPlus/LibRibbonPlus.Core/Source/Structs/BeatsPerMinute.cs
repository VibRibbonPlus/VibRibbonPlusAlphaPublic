// <copyright file="BeatsPerMinute.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents a tempo in music.
/// </summary>
/// <param name="Tempo">The amount of beats within one minute.</param>
/// <param name="Interval">The occurence in time of when the tempo is applied.</param>
[StructLayout(LayoutKind.Auto)]
public record struct BeatsPerMinute(float Tempo, Interval Interval)
    : ITimed
{
    /// <summary>
    /// Contains the separator for a prefix and the raw interval.
    /// </summary>
    public const string Separator = ": ";

    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static BeatsPerMinute Blank { get; } = new(120, default);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{Interval}: {Tempo.ToString(CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Tries to create a new instance of <see cref="BeatsPerMinute"/> from a given <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to parse.</param>
    /// <param name="beatsPerMinute">The result, or <see cref="Blank"/>.</param>
    /// <returns>The value <see langword="true"/> if the parse is successful, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(Chars chars, out BeatsPerMinute beatsPerMinute)
    {
        beatsPerMinute = Blank;

        if (!chars.TrySplit(Separator, out Chars time, out Chars tempo))
            return false;

        if (!float.TryParse(tempo.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float amount))
            return false;

        if (!Interval.TryParse(time.ToString(), out Interval interval))
            return false;

        beatsPerMinute = new(amount, interval);
        return true;
    }
}
