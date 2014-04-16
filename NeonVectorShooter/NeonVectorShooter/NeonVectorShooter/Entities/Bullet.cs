using System;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Contents;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.ParticleSystem;

namespace NeonVectorShooter.Entities
{
    public class Bullet : Entity
    {
        private static readonly Random Random;

        static Bullet()
        {
            Random = new Random();
        }
        public Bullet(Vector2 position, Vector2 velocity)
        {
            Image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = velocity.ToAngle();
            Radius = 8;
        }

        public override void Update(GameTime gameTime)
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            if (GameRoot.Viewport.Bounds.Contains(Position.ToPoint())) return;
            
            IsExpired = true;

            for (var i = 0; i < 30; i++)
                CreateExplosionParticle();
        }

        private void CreateExplosionParticle()
        {
            var state = new ParticleState
            {
                Velocity = Random.NextVector(0, 9),
                Type = ParticleType.Bullet,
                LengthMultiplier = 1
            };
            GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, Color.LightBlue, 50, 1, state);
        }
    }
}