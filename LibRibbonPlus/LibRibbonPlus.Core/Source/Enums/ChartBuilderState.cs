// <copyright file="ChartBuilderState.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents the different states that a <see cref="ChartBuilder"/> can have.
/// </summary>
public enum ChartBuilderState
{
    /// <summary>
    /// Indicates that the <see cref="ChartBuilder"/> is processing the version.
    /// </summary>
    IsProcessingVersion,

    /// <summary>
    /// Indicates that the <see cref="ChartBuilder"/> is processing the metadata.
    /// </summary>
    IsProcessingMetadata,

    /// <summary>
    /// Indicates that the <see cref="ChartBuilder"/> is processing the beats per minute collections.
    /// </summary>
    IsProcessingBeatsPerMinute,

    /// <summary>
    /// Indicates that the <see cref="ChartBuilder"/> is processing the note collections.
    /// </summary>
    IsProcessingNotes,
}
