// <copyright file="NoteCollection.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Encapsulates an <see cref="Array"/> of <see cref="Note"/>.
/// </summary>
/// <param name="Collection">The collection to encapsulate.</param>
public sealed record NoteCollection(Note[] Collection)
    : IEnumerable<Note>
{
    private int _index;

    /// <summary>
    /// Gets the explicit default value of this type.
    /// </summary>
    public static NoteCollection Blank { get; } = new(Array.Empty<Note>());

    /// <summary>
    /// Gets a value indicating whether it isn't currently looking at any note.
    /// </summary>
    public bool IsOutOfRange => _index >= Collection.Length;

    /// <summary>
    /// Gets the current note that this collection is representing.
    /// </summary>
    public Note Current => IsOutOfRange
        ? throw new InvalidOperationException($"Out of max range ({Collection.Length.ToStringInvariant()}): {_index.ToStringInvariant()}")
        : Collection[_index];

    /// <summary>
    /// Moves to the next note in the collection.
    /// </summary>
    /// <returns>The value <see langword="true"/> if there is a next value, otherwise <see langword="false"/>.</returns>
    public bool MoveNext() => ++_index < Collection.Length;

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder builder = new();

        Collection.ForEach(note => builder.Append(note).AppendLine());

        return builder.ToString();
    }

    /// <inheritdoc/>
    public IEnumerator<Note> GetEnumerator() => ((IEnumerable<Note>)Collection).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
}
