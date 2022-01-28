using System.Collections.Generic;
using System.Linq;
using LibRibbonPlus;
using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    public sealed class ShapeCollector : MonoBehaviour
    {
        [SerializeField]
        private List<Shape> _shapes = new();

        internal List<Vector3>? From(Obstacles obstacle)
            => _shapes.FirstOrDefault(s => s.IsMatch(obstacle))?.Vectors;
    }
}
