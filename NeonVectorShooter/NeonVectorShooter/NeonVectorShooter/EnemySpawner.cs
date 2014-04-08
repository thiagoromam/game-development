using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter
{
    public static class EnemySpawner
    {
        private static readonly Random Random;
        private static float _inverseSpawnChance;

        static EnemySpawner()
        {
            Random = new Random();
            _inverseSpawnChance = 60;
        }

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (Random.Next((int)_inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                if (Random.Next((int)_inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));
            }

            if (_inverseSpawnChance > 20)
                _inverseSpawnChance -= 0.05f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 position;
            do
            {
                position = Random.Next(GameRoot.ScreenSize);

            } while (Vector2.DistanceSquared(position, PlayerShip.Instance.Position) < Math.Pow(250, 2));

            return position;
        }

        public static void Reset()
        {
            _inverseSpawnChance = 60;
        }
    }
}