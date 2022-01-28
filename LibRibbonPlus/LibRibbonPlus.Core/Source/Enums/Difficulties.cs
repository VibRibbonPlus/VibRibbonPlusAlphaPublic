// <copyright file="Difficulties.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents all kinds of difficulties, part of chart metadata.
/// </summary>
public enum Difficulties
{
    /// <summary>
    /// Indicates that the chart's difficulty is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Indicates that the chart is easy.
    /// </summary>
    Bronze,

    /// <summary>
    /// Indicates that the chart is medium.
    /// </summary>
    Silver,

    /// <summary>
    /// Indicates that the chart is hard.
    /// </summary>
    Gold,
}
