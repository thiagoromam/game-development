using System;

namespace Exercise3
{
    public static class Log
    {
        public static void Write(string format, Type type, ConsoleColor? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            
            Console.WriteLine(format, Name(type));

            if (color.HasValue)
                Console.ResetColor();
        }
        public static void Write(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        private static string Name(Type type)
        {
            var name = type.Name;

            if (type.GenericTypeArguments.Length > 0)
            {
                name = name.Replace("`1", "");
                name += " -> " + type.GenericTypeArguments[0].Name;
            }

            return name;
        }
    }
}