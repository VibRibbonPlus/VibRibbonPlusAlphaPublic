// <copyright file="Obstacles.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents all kinds of obstacles, part of chart metadata.
/// </summary>
[Flags]
public enum Obstacles
{
    /// <summary>
    /// Indicates that there is no note.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that there is a note of at least type block, which requires the left trigger to be pressed on the controller by default.
    /// </summary>
    Block,

    /// <summary>
    /// Indicates that there is a note of at least type pit, which requires d-pad down to be pressed on the controller by default.
    /// </summary>
    Pit,

    /// <summary>
    /// Indicates that there is a note of at least type wave, which requires the x button to be pressed on the controller by default.
    /// </summary>
    Wave = 1 << 2,

    /// <summary>
    /// Indicates that there is a note of at least type loop, which requires the right trigger to be pressed on the controller by default.
    /// </summary>
    Loop = 1 << 3,
}
