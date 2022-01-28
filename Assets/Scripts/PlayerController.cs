using Emik.Microtypes.Extensions;
using LibRibbonPlus;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// A controller for keeping track of inputs by a player.
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        private PlayerInput _input = new();

        /// <summary>
        /// Invoked on the frame a player presses a button registered by <see cref="PlayerInput"/>.
        /// </summary>
        internal event InputHandler OnPress = _ => { };

        /// <summary>
        /// Invoked on the frame a player releases a button registered in <see cref="PlayerInput"/>.
        /// </summary>
        internal event InputHandler OnRelease = _ => { };

        internal void Update()
        {
            Process(_input.Presses, OnPress);
            Process(_input.Releases, OnRelease);
        }

        private static void Process(Obstacles obstacle, InputHandler handler)
            => obstacle.Split().ForEach(o => handler(o));
    }
}
