// <copyright file="ChartFormatVersion.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Stores a chart format version.
/// </summary>
/// <param name="Major">The version number in major.</param>
public readonly record struct ChartFormatVersion(uint Major)
{
    private const int Latest = 1;

    private const string Prefix = "vib+";

    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static ChartFormatVersion Blank { get; } = new(Latest);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{Prefix}{Major}";

    /// <summary>
    /// Tries to create a new instance of <see cref="ChartFormatVersion"/> from a given <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to parse.</param>
    /// <param name="version">The result, or <see cref="Blank"/>.</param>
    /// <returns>The value <see langword="true"/> if the parse is successful, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(Chars chars, out ChartFormatVersion version)
    {
        if (chars[Prefix.Length..] != Prefix)
        {
            version = Blank;
            return false;
        }

        bool output = uint.TryParse(chars[..Prefix.Length].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out uint result);

        version = new(result);

        return output;
    }
}
