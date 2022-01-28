using UnityEngine;
#nullable enable
namespace VibRibbonPlus
{
    /// <summary>
    /// Represents a relative line, part of the construction of vectors in a <see cref="LineRenderer"/>.
    /// </summary>
    /// <param name="Shake">The amount of shaking.</param>
    /// <param name="Vector">The preexisting shake.</param>
    internal record RelativeLine(float Shake, Vector3 Vector);
}
