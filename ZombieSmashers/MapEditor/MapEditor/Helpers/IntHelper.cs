using System;

namespace MapEditor.Helpers
{
    public static class IntHelper
    {
        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }
    }
}