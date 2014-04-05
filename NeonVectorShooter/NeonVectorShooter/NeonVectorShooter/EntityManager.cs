using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach
namespace NeonVectorShooter
{
    public static class EntityManager
    {
        private static readonly List<Entity> Entities;
        private static readonly List<Entity> AddedEntities;
        private static bool _isUpdating;

        public static int Count
        {
            get { return Entities.Count; }
        }

        static EntityManager()
        {
            Entities = new List<Entity>();
            AddedEntities = new List<Entity>();
        }

        public static void Add(Entity entity)
        {
            if (!_isUpdating)
                Entities.Add(entity);
            else
                AddedEntities.Add(entity);
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