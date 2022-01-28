// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable
namespace System;

/// <summary>
/// Extension methods for Span{T}, Memory{T}, and friends.
/// </summary>
public static class MemoryExtensions
{
    /// <summary>
    /// Creates a new span over the portion of the target array.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this T[]? array, int start)
        => array == null
            ? start != 0
                ? throw new ArgumentOutOfRangeException()
                : default
            : !typeof(T).IsValueType && array.GetType() != typeof(T[])
            ? throw new ArrayTypeMismatchException()
            : (uint)start > (uint)array.Length
            ? throw new ArgumentOutOfRangeException()
            : new Span<T>(array, start, array.Length - start);

    /// <summary>
    /// Creates a new readonly span over the portion of the target string.
    /// </summary>
    /// <param name="text">The target string.</param>
    /// <remarks>Returns default when <paramref name="text"/> is null.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan(this string? text) =>
        text == null
            ? default
            : new ReadOnlySpan<char>(text.ToCharArray());

    /// <summary>
    /// Creates a new readonly span over the portion of the target string.
    /// </summary>
    /// <param name="text">The target string.</param>
    /// <param name="start">The index at which to begin this slice.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="start"/> index is not in range (&lt;0 or &gt;text.Length).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan(this string? text, int start)
        => text == null
                ? start != 0
                    ? throw new ArgumentOutOfRangeException(nameof(start))
                    : default
                : (uint)start > (uint)text.Length
            ? throw new ArgumentOutOfRangeException(nameof(start))
            : new ReadOnlySpan<char>(text.ToCharArray(), start, text.Length - start);

    /// <summary>
    /// Creates a new readonly span over the portion of the target string.
    /// </summary>
    /// <param name="text">The target string.</param>
    /// <param name="start">The index at which to begin this slice.</param>
    /// <param name="length">The desired length for the slice (exclusive).</param>
    /// <remarks>Returns default when <paramref name="text"/> is null.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="start"/> index or <paramref name="length"/> is not in range.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan(this string? text, int start, int length)
    {
        if (text == null)
        {
            return start != 0 || length != 0
                ? throw new ArgumentOutOfRangeException(nameof(start))
                : default;
        }

#if TARGET_64BIT
            // See comment in Span<T>.Slice for how this works.
            if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)text.Length)
                throw new ArgumentOutOfRangeException(nameof(start));
#else
        if ((uint)start > (uint)text.Length || (uint)length > (uint)(text.Length - start))
            throw new ArgumentOutOfRangeException(nameof(start));
#endif

        return new ReadOnlySpan<char>(text.ToCharArray(), start, length);
    }

    /// <summary>
    /// Searches for the specified value and returns true if found. If not found, returns false. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this Span<T> span, T value) where T : IEquatable<T>
    {
        for (int i = 0, n = span._length; i < n; i++)
        {
            if (span[i].Equals(value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for the specified value and returns true if found. If not found, returns false. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
    {
        for (int i = 0, n = span._length; i < n; i++)
        {
            if (span[i].Equals(value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>
    {
        for (int i = 0, n = span._length; i < n; i++)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>
    {
        for (int i = span._length; i >= 0; i--)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>
    {
        if (span._length != other.Length)
            return false;

        for (int i = 0; i < span._length; i++)
        {
            if (!span[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
    {
        for (int i = 0, n = span._length; i < n; i++)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="value">The value to search for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
    {
        for (int i = span._length; i >= 0; i--)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Creates a new span over the target array.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this T[]? array) => new(array);

    /// <summary>
    /// Creates a new Span over the portion of the target array beginning
    /// at 'start' index and ending at 'end' index (exclusive).
    /// </summary>
    /// <param name="array">The target array.</param>
    /// <param name="start">The index at which to begin the Span.</param>
    /// <param name="length">The number of items in the Span.</param>
    /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
    /// <exception cref="ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;Length).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this T[]? array, int start, int length) => new(array, start, length);

    /// <summary>
    /// Creates a new span over the portion of the target array segment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this ArraySegment<T> segment) => new(segment.Array, segment.Offset, segment.Count);

    /// <summary>
    /// Creates a new Span over the portion of the target array beginning
    /// at 'start' index and ending at 'end' index (exclusive).
    /// </summary>
    /// <param name="segment">The target array.</param>
    /// <param name="start">The index at which to begin the Span.</param>
    /// <remarks>Returns default when <paramref name="segment"/> is null.</remarks>
    /// <exception cref="ArrayTypeMismatchException">Thrown when <paramref name="segment"/> is covariant and array's type is not exactly T[].</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;segment.Count).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start)
        => (uint)start > (uint)segment.Count
            ? throw new ArgumentOutOfRangeException(nameof(start))
            : new Span<T>(segment.Array, segment.Offset + start, segment.Count - start);

    /// <summary>
    /// Creates a new Span over the portion of the target array beginning
    /// at 'start' index and ending at 'end' index (exclusive).
    /// </summary>
    /// <param name="segment">The target array.</param>
    /// <param name="start">The index at which to begin the Span.</param>
    /// <param name="length">The number of items in the Span.</param>
    /// <remarks>Returns default when <paramref name="segment"/> is null.</remarks>
    /// <exception cref="ArrayTypeMismatchException">Thrown when <paramref name="segment"/> is covariant and array's type is not exactly T[].</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;segment.Count).
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start, int length)
        => (uint)start > (uint)segment.Count
            ? throw new ArgumentOutOfRangeException(nameof(start))
            : (uint)length > (uint)(segment.Count - start)
            ? throw new ArgumentOutOfRangeException(nameof(length))
            : new Span<T>(segment.Array, segment.Offset + start, length);

    /// <summary>
    /// Copies the contents of the array into the span. If the source
    /// and destinations overlap, this method behaves as if the original values in
    /// a temporary location before the destination is overwritten.
    ///
    ///<param name="source">The array to copy items from.</param>
    /// <param name="destination">The span to copy items into.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the destination Span is shorter than the source array.
    /// </exception>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<T>(this T[]? source, Span<T> destination) => new ReadOnlySpan<T>(source).CopyTo(destination);
}
