// <copyright file="Enums.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Implementations for <see cref="Difficulties"/> and <see cref="AllObstacles"/>.
/// </summary>
public static class Enums
{
    /// <summary>
    /// Gets all of the values of <see cref="AllObstacles"/> individually.
    /// </summary>
    public static Obstacles[] AllObstacles { get; } = (Obstacles[])Enum.GetValues(typeof(Obstacles));

    /// <summary>
    /// Gets the number of obstacles in an instance of <see cref="AllObstacles"/>.
    /// </summary>
    /// <param name="obstacle">The instance of <see cref="AllObstacles"/> to get the obstacles of.</param>
    /// <returns>An <see cref="int"/> representing the number of bits flipped to 1, indicating a presense.</returns>
    public static int Count(this Obstacles obstacle)
    {
        int n = (int)obstacle;

        return n is 0
            ? 0
            : (n & 1) + Count((Obstacles)(n >> 1));
    }

    /// <summary>
    /// Converts a string to <see cref="Difficulties"/>.
    /// </summary>
    /// <param name="difficulty">The <see cref="string"/> representation of <see cref="Difficulties"/> to convert.</param>
    /// <returns>The value of <see cref="Difficulties"/> from its name, or <see cref="Difficulties.Unknown"/>.</returns>
    public static Difficulties ToDifficulties(this string? difficulty) => difficulty switch
    {
        nameof(Difficulties.Bronze) => Difficulties.Bronze,
        nameof(Difficulties.Silver) => Difficulties.Silver,
        nameof(Difficulties.Gold) => Difficulties.Gold,
        _ => Difficulties.Unknown,
    };

    /// <summary>
    /// Splits an <see cref="Obstacles"/> into individual parts.
    /// </summary>
    /// <param name="obstacle">The <see cref="Obstacles"/> to split.</param>
    /// <returns>A collection of <see cref="Obstacles"/> based on the parameter <paramref name="obstacle"/> for each flag that is set <see langword="true"/>.</returns>
    public static IEnumerable<Obstacles> Split(this Obstacles obstacle)
        => AllObstacles.Where(constraint => (obstacle & constraint) is not 0);

    /// <summary>
    /// Extension method of <see cref="TooManyObstaclesException.AssertOnlyOneOf(Obstacles)"/>.
    /// </summary>
    /// <param name="obstacle">The <see cref="Obstacles"/> to assert.</param>
    /// <returns>Itself.</returns>
    public static Obstacles AssertOnlyOneOf(this Obstacles obstacle)
    {
        TooManyObstaclesException.AssertOnlyOneOf(obstacle);
        return obstacle;
    }

    /// <summary>
    /// Converts a string to <see cref="Obstacles"/>.
    /// </summary>
    /// <param name="obstacles">The <see cref="string"/> representation of <see cref="Obstacles"/> to convert.</param>
    /// <returns>The value of <see cref="Obstacles"/> from its name, or <see cref="Obstacles.None"/>.</returns>
    public static Obstacles ToObstacles(this string? obstacles)
    {
        _ = Enum.TryParse(obstacles, out Obstacles output);
        return output;
    }
}
