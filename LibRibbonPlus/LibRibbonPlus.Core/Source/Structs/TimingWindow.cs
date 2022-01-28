// <copyright file="TimingWindow.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Specifies a timing window between two points.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct TimingWindow
{
    private readonly Interval _center;

    private readonly Interval _offset;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimingWindow"/> struct.
    /// </summary>
    /// <param name="center">The center of the timing window.</param>
    /// <param name="offset">The amount of offset allowed by the timing window.</param>
    public TimingWindow(Interval center, Interval offset)
    {
        _center = center;
        _offset = offset;
    }

    /// <summary>
    /// Gets the earliest window in which this <see cref="TimingWindow"/> is valid.
    /// </summary>
    public Interval Earliest => _center - _offset;

    /// <summary>
    /// Gets the latest window in which this <see cref="TimingWindow"/> is valid.
    /// </summary>
    public Interval Latest => _center + _offset;

    /// <summary>
    /// Determines whether an <see cref="Interval"/> is between the earliest and latest timing windows.
    /// </summary>
    /// <param name="interval">The interval to check.</param>
    /// <returns>The value <see langword="true"/> if <see cref="Earliest"/> is less than or equal <paramref name="interval"/> and <paramref name="interval"/> is less than or equal <see cref="Latest"/>.</returns>
    public readonly bool IsBetween(Interval interval) => Earliest <= interval && interval <= Latest;
}
