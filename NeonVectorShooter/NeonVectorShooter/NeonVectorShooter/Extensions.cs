using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter
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

        public static float NextFloat(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}