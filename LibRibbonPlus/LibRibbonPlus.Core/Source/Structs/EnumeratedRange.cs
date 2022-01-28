// <copyright file="EnumeratedRange.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents an enumerable range.
/// </summary>
/// <param name="Start">The index to start.</param>
/// <param name="End">The index to end.</param>
[StructLayout(LayoutKind.Auto)]
public record struct EnumeratedRange(int Start, int End)
{
    private int _current = Start;

    /// <summary>
    /// Moves to the next number, wrapping around back to <see cref="Start"/> if the current value reaches <see cref="End"/>.
    /// </summary>
    /// <returns>The new value of the current value.</returns>
    public int Step()
    {
        if (++_current >= End)
            _current = Start;

        return _current;
    }
}
