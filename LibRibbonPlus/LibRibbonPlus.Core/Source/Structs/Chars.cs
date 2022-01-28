// <copyright file="Chars.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents a span of characters.
/// </summary>
public ref struct Chars
{
    private readonly int _start;

    private readonly int _end;

    private readonly string _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="Chars"/> struct.
    /// </summary>
    /// <param name="collection">The collection of characters to encapsulate.</param>
    public Chars(string? collection)
    {
        _start = 0;

        if (collection is null)
        {
            _end = 0;
            _collection = string.Empty;
            return;
        }

        _collection = collection;
        _end = collection.Length;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Chars"/> struct.
    /// </summary>
    /// <param name="collection">The collection of characters to encapsulate.</param>
    /// <param name="start">The index to treat as the start.</param>
    /// <param name="end">The index to treat as the end.</param>
    public Chars(string? collection, int start, int end)
    {
        if (collection == null)
        {
            _start = 0;
            _end = 0;
            _collection = string.Empty;
            return;
        }

        _start = start;
        _end = end;
        _collection = collection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Chars"/> struct.
    /// </summary>
    /// <param name="collection">The collection of characters to encapsulate.</param>
    /// <param name="start">The index to treat as the start.</param>
    /// <param name="end">The index to treat as the end.</param>
    public Chars(string? collection, Index start, Index end)
        : this(
            collection,
            start.IsFromEnd ? (collection?.Length ?? 0) - start.Value : start.Value,
            end.IsFromEnd ? (collection?.Length ?? 0) - end.Value : end.Value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Chars"/> struct.
    /// </summary>
    /// <param name="collection">The collection of characters to encapsulate.</param>
    /// <param name="range">The range of indices to use.</param>
    public Chars(string? collection, Range range)
        : this(collection, range.Start, range.End)
    {
    }

    /// <summary>
    /// Gets a representation of an empty span.
    /// </summary>
    public static Chars Empty => new(string.Empty);

    /// <summary>
    /// Gets a value indicating whether the length is 0.
    /// </summary>
    public bool IsEmpty => Length is 0;

    /// <summary>
    /// Gets the length of the span.
    /// </summary>
    public int Length => _end - _start - 1;

    /// <summary>
    /// Index into this instance of <see cref="Chars"/>.
    /// </summary>
    /// <param name="index">The <see cref="int"/> to index.</param>
    /// <returns>The <see cref="char"/> which represents the index of this <see cref="Chars"/>.</returns>
    public readonly char this[int index] => _collection[_start + index];

    /// <summary>
    /// Index into this instance of <see cref="Chars"/>.
    /// </summary>
    /// <param name="index">The <see cref="Index"/> to index.</param>
    /// <returns>The <see cref="char"/> which represents the index of this <see cref="Chars"/>.</returns>
    public readonly char this[Index index] => _collection[_start + index.Value];

    /// <summary>
    /// Index into this instance of <see cref="Chars"/>.
    /// </summary>
    /// <param name="range">The <see cref="Range"/> to index.</param>
    /// <returns>A sub-<see cref="Chars"/> which represents the range of this <see cref="Chars"/>.</returns>
    public readonly Chars this[Range range] => new(_collection,
        range.Start.IsFromEnd ? _end - range.Start.Value : range.Start.Value,
        range.End.IsFromEnd ? _end - range.End.Value : range.End.Value);

    /// <summary>
    /// Converts a <see cref="string"/> to <see cref="Chars"/>.
    /// </summary>
    /// <param name="chars">The sequence of characters to convert.</param>
    public static implicit operator Chars(string? chars) => new(chars);

    /// <summary>
    /// Converts <see cref="Chars"/> to <see cref="string"/>. This allocates memory on the heap.
    /// </summary>
    /// <param name="chars">The sequence of characters to convert.</param>
    public static explicit operator string(Chars chars) => chars.ToString();

    /// <summary>
    /// Compares two <see cref="Chars"/> to see if they contain the same sequence.
    /// </summary>
    /// <param name="left">The first <see cref="Chars"/> to compare.</param>
    /// <param name="right">The second <see cref="Chars"/> to compare.</param>
    /// <returns>The value <see langword="true"/> if <see cref="Chars"/> contains the same slice of characters, otherwise <see langword="false"/>.</returns>
    public static bool operator ==(Chars left, Chars right)
    {
        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
            if (left[i] == right[i])
                return false;

        return true;
    }

    /// <summary>
    /// Compares two <see cref="Chars"/> to see if they don't contain the same sequence.
    /// </summary>
    /// <param name="left">The first <see cref="Chars"/> to compare.</param>
    /// <param name="right">The second <see cref="Chars"/> to compare.</param>
    /// <returns>The value <see langword="false"/> if <see cref="Chars"/> contains the same slice of characters, otherwise <see langword="true"/>.</returns>
    public static bool operator !=(Chars left, Chars right) => !(left == right);

    /// <inheritdoc/>
    public override readonly bool Equals(object obj) => false;

    /// <summary>
    /// Splits <see cref="Chars"/> into two based on the first find of a separator sequence. Neither <see cref="Chars"/> include this sequence.
    /// </summary>
    /// <param name="separator">The separator of the two outputted <see cref="Chars"/>.</param>
    /// <param name="before">The sequence of characters before <paramref name="separator"/>.</param>
    /// <param name="after">The sequence of characters after <paramref name="separator"/>.</param>
    /// <returns>The value <see langword="true"/> if <paramref name="separator"/> is part of this instance, otherwise <see langword="false"/>.</returns>
    public readonly bool TrySplit(Chars separator, out Chars before, out Chars after)
    {
        int index = _collection.IndexOf(separator._collection, StringComparison.Ordinal);

        if (index is -1)
        {
            before = Empty;
            after = Empty;
            return false;
        }

        before = this[..index];
        after = this[(index + separator.Length + 1)..];
        return true;
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(_start, _end, _collection);

    /// <inheritdoc/>
    public override readonly string ToString()
        => _start is 0 && _end == _collection.Length
            ? _collection
            : _collection[_start.._end];

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public readonly Enumerator GetEnumerator() => new(this);

    /// <summary>
    /// An enumerator for this type.
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public ref struct Enumerator
    {
        private readonly Chars _chars;

        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumerator"/> struct.
        /// </summary>
        /// <param name="chars">The span of characters to encapsulate.</param>
        internal Enumerator(Chars chars)
        {
            _chars = chars;
            _index = -1;
        }

        /// <summary>
        /// Gets the current <see cref="char"/>.
        /// </summary>
        public readonly char Current => _chars[_index];

        /// <summary>
        /// Goes to the next index, returns whether or not it can.
        /// </summary>
        /// <returns>The value <see langword="true"/> if there is a next character, else <see langword="false"/>.</returns>
        public bool MoveNext()
        {
            int index = _index + 1;

            if (index >= _chars.Length)
                return false;

            _index = index;

            return true;
        }
    }
}
