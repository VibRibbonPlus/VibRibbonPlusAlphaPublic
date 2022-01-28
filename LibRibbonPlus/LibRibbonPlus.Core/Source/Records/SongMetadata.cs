// <copyright file="SongMetadata.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents metadata that is strictly related to the song.
/// </summary>
/// <param name="Title">The title of the song.</param>
/// <param name="Artist">The artist of the song.</param>
/// <param name="Directory">The relative directory to a file which contains the song.</param>
public sealed record SongMetadata(Label Title, Label Artist, Directory Directory)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static SongMetadata Blank { get; } = new(Label.Blank, Label.Blank, Directory.Blank);
}
