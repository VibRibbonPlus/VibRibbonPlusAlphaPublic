// <copyright file="ITimed.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// An interface which contains data that indicates an event in a specific time.
/// </summary>
public interface ITimed
{
    /// <summary>
    /// Gets the interval in which something happens.
    /// </summary>
    public Interval Interval { get; }
}
