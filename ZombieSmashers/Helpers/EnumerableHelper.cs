using System.Linq;

namespace Helpers
{
    public static class EnumerableHelper
    {
        public static T[] Array<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(i => new T()).ToArray();
        }
        public static T[] Array<T>(int count, T commonValue)
        {
            return Enumerable.Range(0, count).Select(i => commonValue).ToArray();
        }
    }
}
