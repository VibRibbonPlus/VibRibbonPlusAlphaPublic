using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Emik.Microtypes.Unity;
using LibRibbonPlus;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Handles rendering the playfield which is the massive line at the bottom.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(ShapeCollector))]
    public sealed class LineController : CacheableBehaviour, IChartExtractor
    {
        [SerializeField]
        private float _shakeIntensity;

        [SerializeField]
        private float _visualOffset;

        private NoteQueue? _queue;

        internal TimingWindow Now => new(Find<Timer>().Elapsed, Interval.Rendering);

        private const float MaxRenderLength = 20;

        private const int Sections = 20;

        /// <inheritdoc/>
        public void Set(Chart chart) => _queue = new(chart.Notes.GetEnumerator());

        internal void Update()
        {
            if (_queue is null)
                return;

            _queue.Refresh(Now);

            Interval now = Find<Timer>().Elapsed;

            ShapeCollector instance = Get<ShapeCollector>();

            if (!instance || _queue.Count is 0)
                return;

            Vector3[] array = _queue
                .Select(n => new AbsoluteLine(n.IncreasedShake, DistanceFromTime(n, now), instance.From(n.Kind)))
                .OrderBy(t => t.X)
                .Aggregate(new List<RelativeLine>() { new(0, new(-MaxRenderLength, 0)) }, LineAggregator)
                .Append(new(0, new(MaxRenderLength, 0)))
                .AddSegments(Sections, MaxRenderLength)
                .Where(v => v.Vector.x is >= -MaxRenderLength and <= MaxRenderLength)
                .Select(v => v.Vector + Next(v.Shake) + Next(_shakeIntensity))
                .ToArray();

            Get<LineRenderer>().positionCount = array.Length;
            Get<LineRenderer>().SetPositions(array);
        }

        /// <summary>
        /// Sets the shake intensity which is a value added to each vector in random quantities.
        /// </summary>
        /// <param name="value">The amount of intensity.</param>
        internal void SetShakeIntensity(float value)
        {
            _shakeIntensity = value;
            Debug.Log("Updated: " + value);
        }

        private float DistanceFromTime(Note n, Interval now)
            => ((float)(n.Interval - now + (Interval)_visualOffset)) * n.Velocity.Amount * MaxRenderLength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<RelativeLine> LineAggregator(List<RelativeLine> output, AbsoluteLine next)
        {
            if (next.List is not null)
                output.AddRange(next.List.Select(v => new RelativeLine(next.Shake, v + new Vector3(next.X, 0))));

            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 Next(float f) => new(Random.Range(-f, f), Random.Range(-f, f), 0);

        private record AbsoluteLine(float Shake, float X, List<Vector3>? List);
    }
}
