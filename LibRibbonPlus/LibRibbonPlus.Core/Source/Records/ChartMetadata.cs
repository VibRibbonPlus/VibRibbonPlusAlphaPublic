// <copyright file="ChartMetadata.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents metadata that is strictly related to the chart.
/// </summary>
/// <param name="Charter">Credit to whoever charted the chart.</param>
/// <param name="Difficulty">The difficulty of the chart.</param>
public sealed record ChartMetadata(Label Charter, Difficulties Difficulty)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static ChartMetadata Blank { get; } = new(Label.Blank, Difficulties.Unknown);
}
