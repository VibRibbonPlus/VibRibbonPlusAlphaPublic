// <copyright file="IChartExtractor.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// An interface to indicate something that can extract information from a chart.
/// </summary>
public interface IChartExtractor
{
    /// <summary>
    /// The method to extract data from a given chart.
    /// </summary>
    /// <param name="chart">The chart to extract data from.</param>
    public void Set(Chart chart);
}
