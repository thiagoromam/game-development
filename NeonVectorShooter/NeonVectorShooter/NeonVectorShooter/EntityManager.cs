using System.Collections.Generic;
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

            for (var i = 0; i < Entities.Count; ++i)
                Entities[i].Update();

            _isUpdating = false;

            Entities.AddRange(AddedEntities);
            AddedEntities.Clear();

            Entities.RemoveAll(e => e.IsExpired);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < Entities.Count; ++i)
                Entities[i].Draw(spriteBatch);
        }
    }
}