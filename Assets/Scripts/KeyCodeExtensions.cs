using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Extension methods for the type <see cref="KeyCode"/>.
    /// </summary>
    internal static class KeyCodeExtensions
    {
        private static Dictionary<string, KeyCode> s_keyCodes = ((KeyCode[])Enum
            .GetValues(typeof(KeyCode)))
            .ToDictionary(k => k.ToString(), k => k);

        /// <summary>
        /// Converts a <see cref="string"/> to an equivalent <see cref="KeyCode"/>, or <see cref="KeyCode.None"/>.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <returns>A <see cref="KeyCode"/> that contains the same name as <paramref name="input"/>.</returns>
        internal static KeyCode ToKeyCode(this string input) => s_keyCodes.GetValueOrDefault(input);
    }
}
