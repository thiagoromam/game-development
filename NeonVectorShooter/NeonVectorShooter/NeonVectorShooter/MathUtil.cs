using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter
{
    public static class MathUtil
    {
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * magnitude;
        }
    }
}