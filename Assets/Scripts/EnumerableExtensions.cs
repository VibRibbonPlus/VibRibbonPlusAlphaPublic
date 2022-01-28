using System.Collections.Generic;
#nullable enable
namespace VibRibbonPlus
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<RelativeLine> AddSegments(this IEnumerable<RelativeLine> enumerable, int count, float max)
        {
            IEnumerator<RelativeLine> enumerator = enumerable.GetEnumerator();

            float current = -max / 2,
                delta = max / (count / 2);

            RelativeLine previous = new(0, default);

            while (enumerator.MoveNext())
            {
                while (enumerator.Current.Vector.x > current + delta)
                {
                    current += delta;

                    if (enumerator.Current.Vector.y is 0 && previous.Vector.y is 0)
                        yield return new(0, new(current, 0));
                }

                if (enumerator.Current.Vector.x > current + delta)
                    current += delta;

                previous = enumerator.Current;
                yield return enumerator.Current;
            }
        }
    }
}
