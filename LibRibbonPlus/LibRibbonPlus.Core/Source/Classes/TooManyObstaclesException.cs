// <copyright file="TooManyObstaclesException.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Specifies that <see cref="Obstacles"/> has multiple values when one is expected.
/// </summary>
public class TooManyObstaclesException : Exception
{
    private TooManyObstaclesException(Obstacles obstacle, int maxCount, int count)
        : base($"Expected maximum count {maxCount.ToStringInvariant()}, received {count.ToStringInvariant()} ({obstacle}).")
    {
    }

    /// <summary>
    /// Asserts that an instance of <see cref="Obstacles"/> has only one value.
    /// </summary>
    /// <param name="obstacle">The <see cref="Obstacles"/> to assert.</param>
    public static void AssertOnlyOneOf(Obstacles obstacle)
    {
        const int Single = 1;

        if (obstacle.Count() is not Single and int count)
            throw new TooManyObstaclesException(obstacle, Single, count);
    }
}
