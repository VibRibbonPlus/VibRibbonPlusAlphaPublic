using System;
using System.Collections.Generic;
using LibRibbonPlus;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
#nullable enable
namespace VibRibbonPlus
{
    [Serializable]
    internal sealed class Shape
    {
        [field: SerializeField]
        private Obstacles _obstacle;

        [field: SerializeField]
        internal List<Vector3> Vectors { get; private set; } = new();

        internal bool IsMatch(Obstacles obstacles) => obstacles == _obstacle;
    }
}
