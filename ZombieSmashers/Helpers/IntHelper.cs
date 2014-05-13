using System;

namespace Helpers
{
    public static class IntHelper
    {
        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static bool Between(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}