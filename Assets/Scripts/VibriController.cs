using Emik.Microtypes.Unity;
using LibRibbonPlus;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// A controller for handling animations for the player model.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public sealed class VibriController : CacheableBehaviour
    {
        private BeatsPerMinute _speed = new(120, default);

        internal void OnEnable() => Find<PlayerController>().OnPress += Animate;

        internal void OnDisable() => Find<PlayerController>().OnPress -= Animate;

        internal void Update() => Get<Animator>().speed = _speed.Tempo / 98f;

        /// <summary>
        /// Changes the speed of the animation.
        /// </summary>
        /// <param name="beatsPerMinute">The speed of the animation.</param>
        internal void ChangeSpeed(BeatsPerMinute beatsPerMinute) => _speed = beatsPerMinute;

        private void Animate(Obstacles input) => Get<Animator>().Play(input.ToString(), -1, 0f);
    }
}

