using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach
namespace NeonVectorShooter
{
    public static class EntityManager
    {
        private static readonly List<Enemy> Enemies;
        private static readonly List<Bullet> Bullets; 
        private static readonly List<Entity> Entities;
        private static readonly List<Entity> AddedEntities;
        private static bool _isUpdating;

        public static int Count
        {
            get { return Entities.Count; }
        }

        static EntityManager()
        {
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            Entities = new List<Entity>();
            AddedEntities = new List<Entity>();
        }

        public static void Add(Entity entity)
        {
            if (!_isUpdating)
                AddEntity(entity);
            else
                AddedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            Entities.Add(entity);

            var enemy = entity as Enemy;
            if (enemy != null)
            {
                Enemies.Add(enemy);
                return;
            }

            var bullet = entity as Bullet;
            if (bullet != null) 
                Bullets.Add(bullet);
        }

        public static void Update()
        {
            _isUpdating = true;

            HandleCollisions();

            for (var i = 0; i < Entities.Count; ++i)
                Entities[i].Update();

            _isUpdating = false;

            AddedEntities.ForEach(AddEntity);
            AddedEntities.Clear();

            Entities.RemoveAll(e => e.IsExpired);
            Enemies.RemoveAll(e => e.IsExpired);
            Bullets.RemoveAll(e => e.IsExpired);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < Entities.Count; ++i)
                Entities[i].Draw(spriteBatch);
        }

        public static bool IsColliding(Entity a, Entity b)
        {
            if (a.IsExpired || b.IsExpired)
                return false;

            var radius = a.Radius + b.Radius;
            return Vector2.DistanceSquared(a.Position, b.Position) < Math.Pow(radius, 2);
        }

        public static void HandleCollisions()
        {
            for (var i = 0; i < Enemies.Count; i++)
            {
                for (var j = i + 1; j < Enemies.Count; j++)
                {
                    if (!IsColliding(Enemies[i], Enemies[j])) continue;
                    Enemies[i].HandleCollision(Enemies[j]);
                    Enemies[j].HandleCollision(Enemies[i]);
                }
            }

            for (var i = 0; i < Enemies.Count; i++)
            {
                for (var j = i + 1; j < Bullets.Count; j++)
                {
                    if (!IsColliding(Enemies[i], Bullets[j])) continue;
                    Enemies[i].WasShot();
                    Bullets[j].IsExpired = true;
                }
            }

            if (Enemies.Any(e => e.IsActive && IsColliding(e, PlayerShip.Instance)))
            {
                PlayerShip.Instance.Kill();
                EnemySpawner.Reset();
                Enemies.ForEach(e => e.WasShot());
            }
        }
    }
}