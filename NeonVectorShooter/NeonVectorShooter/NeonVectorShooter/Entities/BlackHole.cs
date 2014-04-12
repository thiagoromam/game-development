using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Contents;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.Managers;

namespace NeonVectorShooter.Entities
{
    public class BlackHole : Entity
    {
        private static Random _random = new Random();

        private int _hitPoints = 10;

        public BlackHole(Vector2 position)
        {
            Image = Art.BlackHole;
            Position = position;
            Radius = Image.Width / 2f;
        }

        public void WasShot()
        {
            if (--_hitPoints <= 0)
                IsExpired = true;
        }

        public void Kill()
        {
            _hitPoints = 0;
            WasShot();
        }

        public override void Update(GameTime gameTime)
        {
            Scale = 1 + 0.1f * (float)Math.Sin(10 * gameTime.TotalGameTime.TotalSeconds);

            var entities =
                from e in EntityManager.GetNerbyEntities(Position, 250)
                let enemy = e as Enemy
                where enemy == null || enemy.IsActive
                select e;

            foreach (var entity in entities)
            {
                Vector2 velocity;
                if (entity is Bullet)
                {
                    velocity = (entity.Position - Position).ScaleTo(0.3f);
                }
                else
                {
                    var diference = Position - entity.Position;
                    var length = diference.Length();

                    velocity = diference.ScaleTo(MathHelper.Lerp(2, 0, length / 250f));
                }

                entity.Velocity += velocity;
            }
        }
    }
}