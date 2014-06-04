using System;
using Microsoft.Xna.Framework;

namespace ZombieSmashers
{
    public static class Rand
    {
        public static Random Random { get; private set; }

        static Rand()
        {
            Random = new Random();
        }

        public static float GetRandomFloat(float min, float max)
        {
            return (float)Random.NextDouble() * (max - min) + min;
        }

        public static double GetRandomDouble(double min, double max)
        {
            return Random.NextDouble() * (max - min) + min;
        }

        public static Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector2(GetRandomFloat(xMin, xMax), GetRandomFloat(yMin, yMax));
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }
    }
}