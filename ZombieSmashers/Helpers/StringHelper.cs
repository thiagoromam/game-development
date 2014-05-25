namespace Helpers
{
    public static class StringHelper
    {
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}