using System.Linq;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            return Shuffle(enumerable, new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random random)
        {
            IEnumerable<T> ts = enumerable as T[] ?? enumerable.ToArray();
            IEnumerable<T> shuffledList = 
                ts.
                    Select(x => new { Number = random.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(ts.Count()); // Assume first @size items is fine

            return shuffledList.ToList();
        }
    }
}
