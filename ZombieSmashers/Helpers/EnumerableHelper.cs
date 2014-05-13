using System.Linq;

namespace Helpers
{
    public static class EnumerableHelper
    {
        public static T[] Array<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(i => new T()).ToArray();
        } 
    }
}
