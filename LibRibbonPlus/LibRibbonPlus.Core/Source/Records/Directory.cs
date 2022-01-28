// <copyright file="Directory.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents a song directory.
/// </summary>
/// <param name="Path">The path to the song.</param>
/// <param name="IsRelative">Determines whether this path is relative or absolute.</param>
public sealed record Directory(string Path, bool IsRelative = true)
{
    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static Directory Blank { get; } = new(string.Empty);

    /// <inheritdoc/>
    public override string ToString() => Path;

    /// <summary>
    /// Reads a file asyncronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> to open and read a file.</returns>
    public async Task<string?> ReadAsync()
    {
        try
        {
            string path = Path;

            using StreamReader reader = File.OpenText(path);

            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }
        catch (Exception ex) when (IsHandled(ex))
        {
            return null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsHandled(Exception ex)
        => ex is ArgumentOutOfRangeException
              or IOException
              or InvalidOperationException
              or NotSupportedException
              or UnauthorizedAccessException;
}
