using System;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Entities;
using NeonVectorShooter.Helpers;

namespace NeonVectorShooter.Managers
{
    public static class EnemySpawner
    {
        private static readonly Random Random;
        private static float _inverseSpawnEnemyChance;
        private const int InverseSpawnBlackHoleChance = 600;

        static EnemySpawner()
        {
            Random = new Random();
            _inverseSpawnEnemyChance = 90;
        }

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (Random.Next((int)_inverseSpawnEnemyChance) == 0)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                if (Random.Next((int)_inverseSpawnEnemyChance) == 0)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));

                if (EntityManager.BlackHoles.Count < 2 && Random.Next(InverseSpawnBlackHoleChance) == 0)
                    EntityManager.Add(new BlackHole(GetSpawnPosition()));
            }

            if (_inverseSpawnEnemyChance > 30)
                _inverseSpawnEnemyChance -= 0.005f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 position;
            do
            {
                position = Random.NextVector(GameRoot.ScreenSize);

            } while (Vector2.DistanceSquared(position, PlayerShip.Instance.Position) < Math.Pow(250, 2));

            return position;
        }

        public static void Reset()
        {
            _inverseSpawnEnemyChance = 90;
        }
    }
}