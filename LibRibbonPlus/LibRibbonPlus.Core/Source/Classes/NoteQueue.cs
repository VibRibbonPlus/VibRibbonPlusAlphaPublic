// <copyright file="NoteQueue.cs" company="RedHead">
// Copyright (c) RedHead. All rights reserved.
// </copyright>

namespace LibRibbonPlus;

/// <summary>
/// Represents a queue of notes used for rendering.
/// </summary>
public sealed class NoteQueue : IEnumerable<Note>
{
    private readonly IEnumerator<Note> _notes;

    private readonly Queue<Note> _queue = new();

    private bool _finished;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoteQueue"/> class.
    /// </summary>
    /// <param name="notes">The notes to queue.</param>
    public NoteQueue(IEnumerator<Note> notes)
    {
        _notes = notes;
        _notes.MoveNext();
    }

    /// <summary>
    /// Gets the number of items in the queue.
    /// </summary>
    public int Count => _queue.Count;

    /// <inheritdoc/>
    public IEnumerator<Note> GetEnumerator() => ((IEnumerable<Note>)_queue).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_queue).GetEnumerator();

    /// <summary>
    /// Refreshes the queue with notes within the window specified.
    /// </summary>
    /// <param name="window">The timing window in which to remove or append notes.</param>
    public void Refresh(TimingWindow window)
    {
        RemoveOldNotes(window);
        AddNewNotes(window);
    }

    private void AddNewNotes(TimingWindow window)
    {
        if (_finished)
            return;

        while (!_finished && window.IsBetween(_notes.Current.Interval))
        {
            _queue.Enqueue(_notes.Current);

            if (!_notes.MoveNext())
                _finished = true;
        }
    }

    private void RemoveOldNotes(TimingWindow window)
    {
        while (true)
        {
            if (_queue.Count is 0)
                break;

            if (window.IsBetween(_queue.Peek().Interval))
                break;

            _queue.Dequeue();
        }
    }
}
