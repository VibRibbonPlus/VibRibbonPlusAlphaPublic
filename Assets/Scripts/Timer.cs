using System;
using LibRibbonPlus;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Keeps track of elapsed time for charts, only exposing an <see cref="Interval"/> object.
    /// </summary>
    public sealed class Timer : MonoBehaviour, IChartExtractor
    {
        private float _elapsed;

        /// <summary>
        /// Invoked when <see cref="Set(Interval)"/> is called.
        /// </summary>
        internal event Action OnSet = () => { };

        /// <summary>
        /// Gets the amount of time elapsed since <see cref="Set(Interval)"/> was called.
        /// </summary>
        internal Interval Elapsed => (Interval)_elapsed;

        /// <inheritdoc/>
        public void Set(Chart chart) => Set();

        internal void Update() => _elapsed += Time.deltaTime;

        /// <summary>
        /// Sets the timer.
        /// </summary>
        /// <param name="offset">The interval to see the timer to.</param>
        internal void Set(Interval offset = default)
        {
            _elapsed = (float)offset;
            OnSet();
        }
    }
}
