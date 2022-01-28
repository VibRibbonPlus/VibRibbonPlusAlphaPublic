using System;
using Emik.Microtypes.Unity;
using LibRibbonPlus;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// A controller for keeping track of notes in a chart.
    /// </summary>
    public sealed class NoteController : CacheableBehaviour, IChartExtractor
    {
        private NoteCollection _notes = new(Array.Empty<Note>());

        private TimingWindow? Next => _notes.IsOutOfRange
            ? null
            : new(_notes.Current.Interval, Interval.Leniency);

        /// <summary>
        /// Invoked whenever the player hits a note correctly.
        /// </summary>
        internal event NoteHandler OnHit = _ => { };

        /// <summary>
        /// Invoked whenever the player misses a note, either through being past <see cref="Interval.Leniency"/> after the initial timing of a note, or by pressing the incorrect button of a note.
        /// </summary>
        internal event NoteHandler OnMiss = _ => { };

        /// <inheritdoc/>
        public void Set(Chart chart) => _notes = chart.Notes;

        internal void OnDisable() => Find<PlayerController>().OnPress -= OnPress;

        internal void OnEnable() => Find<PlayerController>().OnPress += OnPress;

        internal void Update()
        {
            bool flag = false;

            do
            {
                flag = IsPast(t => t.Latest);
                if (flag)
                    MoveNext(false);
            } while (flag);
        }

        private void MoveNext(bool hasClearedNote)
        {
            (hasClearedNote ? OnHit : OnMiss)(_notes.Current);
            _notes.MoveNext();
        }

        private void OnPress(Obstacles input)
        {
            if (IsPast(t => t.Earliest))
                MoveNext(input == _notes.Current.Kind);
        }

        private bool IsPast(Converter<TimingWindow, Interval> func)
            => Next is { } window && func(window) < Find<Timer>().Elapsed;
    }
}
