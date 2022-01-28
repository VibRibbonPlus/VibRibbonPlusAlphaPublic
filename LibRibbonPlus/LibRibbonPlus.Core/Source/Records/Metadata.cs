// <copyright file="Metadata.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents data of a chart other than the chart itself.
/// </summary>
/// <param name="Song">Song-related metadata.</param>
/// <param name="Chart">Chart-related metadata.</param>
/// <param name="Version">The version of the format that was used to create this instance.</param>
public sealed record Metadata(SongMetadata Song, ChartMetadata Chart, ChartFormatVersion Version)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Metadata Blank { get; } = new(SongMetadata.Blank, ChartMetadata.Blank, ChartFormatVersion.Blank);
}
