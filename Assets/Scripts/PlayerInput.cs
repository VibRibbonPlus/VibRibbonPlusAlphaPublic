using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LibRibbonPlus;
using Newtonsoft.Json;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Manages and converts <see cref="KeyCode"/> to the main action type <see cref="Obstacles"/>.
    /// </summary>
    internal sealed class PlayerInput
    {
        private static Dictionary<KeyCode, Obstacles> s_default = new()
        {
            { KeyCode.D, Obstacles.Block },
            { KeyCode.C, Obstacles.Pit },
            { KeyCode.M, Obstacles.Wave },
            { KeyCode.K, Obstacles.Loop },
        };

        /// <summary>
        /// The offset that each chart should start with.
        /// </summary>
        internal Interval Offset { get; set; }

        private Dictionary<KeyCode, Obstacles> _inputs = s_default.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        /// <summary>
        /// Gets the current keys that are pressed from the last frame.
        /// </summary>
        internal Obstacles Presses => ProcessKeyCodes(Input.GetKeyDown);

        /// <summary>
        /// Gets the current keys that are released from the last frame.
        /// </summary>
        internal Obstacles Releases => ProcessKeyCodes(Input.GetKeyUp);

        /// <summary>
        /// Toggles a <see cref="KeyCode"/> off of a specific <see cref="Obstacles"/>.
        /// </summary>
        /// <param name="key">The <see cref="KeyCode"/> to assign.</param>
        /// <param name="obstacle">The <see cref="Obstacles"/> to toggle. This value is assumed to be singular.</param>
        /// <exception cref="TooManyObstaclesException">The value <paramref name="obstacle"/> contains more than one bit enabled.</exception>
        internal void Toggle(KeyCode key, Obstacles obstacle) => _inputs[key] ^= obstacle.AssertOnlyOneOf();

        /// <summary>
        /// Gets the inputs associated with a specific <see cref="KeyCode"/>.
        /// </summary>
        /// <param name="key">The <see cref="KeyCode"/> to get the inputs of.</param>
        /// <returns>The <see cref="Obstacles"/> representing the inputs that <paramref name="key"/> is mapped to. This value may have multiple bits enabled.</returns>
        internal Obstacles Get(KeyCode key) => _inputs.GetValueOrDefault(key);

        private Obstacles ProcessKeyCodes(Predicate<KeyCode> predicate)
            => (Obstacles)_inputs
                .Where(kvp => predicate(kvp.Key))
                .Sum(kvp => (int)kvp.Value);

        /// <summary>
        /// Reads and applies a file's contents into the inputs dictionary asyncronously.
        /// </summary>
        /// <param name="directory">The directory to read from.</param>
        /// <returns>The value <see cref="true"/> if the dictionary was applied successfully, otherwise <see cref="false"/>.</returns>
        internal async Task<bool> ReadAsync(Directory directory)
            => await directory.ReadAsync() is string contents && Read(contents);

        /// <summary>
        /// Reads contents of a serialized dictionary.
        /// </summary>
        /// <param name="contents">The contents to read.</param>
        /// <returns>The value <see cref="true"/> if the dictionary was applied successfully, otherwise <see cref="false"/>.</returns>
        internal bool Read(string contents)
        {
            if (Deserialize(contents) is { } dictionary)
            {
                _inputs = dictionary.ToDictionary(s => s.Value.ToKeyCode(), s => s.Key.ToObstacles());
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override string ToString() => JsonConvert.SerializeObject(_inputs
            .Select(kvp => (Key: kvp.ToString(), kvp.Value))
            .ToDictionary(t => t.Key, t => t.Value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Dictionary<string, string>? Deserialize(string value)
            => JsonConvert.DeserializeObject<Dictionary<string, string>>(value, new JsonSerializerSettings() { Error = (s, a) => { } });
    }
}
