using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emik.Microtypes.Extensions;
using Emik.Microtypes.Unity;
using LibRibbonPlus;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Tests charting capabilities.
    /// </summary>
    public sealed class ChartTester : CacheableBehaviour
    {
        [Range(0, 2)]
        [SerializeField]
        private float _limit;

        [SerializeField]
        private LineController? _line;

        internal void Start()
        {
            AssertChartBuilder();
            AssertNoteController();
        }

        internal void OnGUI() => GUI.Label(new Rect(0, 0, 100, 100), $"FPS: {(int)(1 / Time.smoothDeltaTime)}");

        private void AssertNoteController()
        {
            if (!_line)
                _line = Find<LineController>();

            Find<NoteController>().OnMiss += async e =>
            {
                Debug.Log($"You missed the note: {e}");

                float f = 0;

                while (f < _limit)
                {
                    e.IncreasedShake = (_limit - f) / 2;

                    await Task.Yield();

                    f += Time.deltaTime;
                }
            };

            NoteCollection notes = new(100_000
                .For(i => new Note(
                    Enums.AllObstacles[Random.Range(1, 5)],
                    new(Random.Range(0.25f, 0.5f)),
                    new(((long)i * 1_000_000) + 5_000_000)))
                .ToArray());

            Chart chart = new(notes, new(Array.Empty<BeatsPerMinute>()),
                new(new(new("Title"), new("Artist"), new("Directory")), new(new("Charter"), Difficulties.Silver), new(1)));

            Debug.Log($"Chart contents:\n{chart}");

            Finds<Component>()
                .OfType<IChartExtractor>()
                .ForEach(c => c.Set(chart));
        }

        private Chart AssertChartBuilder()
        {
            IEnumerable<Note> notes = 15.For(i => new Note((Obstacles)Random.Range(0, 15), new(1), new((long)i * (long)Random.Range(1_000_000, 2_000_000))));

            Chart chart = new ChartBuilder()
                .WithArtist(new Label("Artist"))
                .WithCharter(new Label("Charter"))
                .WithVersion(new ChartFormatVersion(1))
                .WithDifficulty(Difficulties.Gold)
                .WithDirectory(new Directory("song.mp3"))
                .WithTitle(new Label("Title"))
                .And(notes)
                .Build();

            Assert.IsNotNull(chart);

            return chart;
        }
    }
}
#endif
