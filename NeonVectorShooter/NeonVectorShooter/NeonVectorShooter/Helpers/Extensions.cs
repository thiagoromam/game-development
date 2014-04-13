using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter.Helpers
{
    public static class Extensions
    {
        public static float ToAngle(this Vector2 value)
        {
            return (float)Math.Atan2(value.Y, value.X);
        }

        public static Point ToPoint(this Vector2 value)
        {
            return new Point((int)value.X, (int)value.Y);
        }

        public static Vector2 ScaleTo(this Vector2 value, float length)
        {
            return value * (length / value.Length());
        }

        public static float NextFloat(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static Vector2 Next(this Random random, Vector2 vector)
        {
            return new Vector2(random.Next((int)vector.X), random.Next((int)vector.Y));
        }
    }
}