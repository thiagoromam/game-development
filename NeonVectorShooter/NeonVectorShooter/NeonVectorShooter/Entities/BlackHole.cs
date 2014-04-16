using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Contents;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.Managers;
using NeonVectorShooter.ParticleSystem;

namespace NeonVectorShooter.Entities
{
    public class BlackHole : Entity
    {
        private static readonly Random Random = new Random();
        private int _hitPoints = 10;
        private float sprayAngle = 0;

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

            CreateExplosion();
        }

        private void CreateExplosion()
        {
            var hue = (float)((3 * GameRoot.GameTime.TotalGameTime.TotalSeconds) % 6);
            var color = ColorUtil.HsvToColor(hue, 0.25f, 1);
            const int numParticles = 150;
            var startOffset = Random.NextFloat(0, MathHelper.TwoPi / numParticles);

            for (var i = 0; i < numParticles; i++)
            {
                var sprayVelocity = MathUtil.FromPolar(MathHelper.TwoPi * i / numParticles + startOffset, Random.NextFloat(8, 16));
                var position = Position + 2f * sprayVelocity;
                var state = new ParticleState
                {
                    Velocity = sprayVelocity,
                    Type = ParticleType.IgnoreGravity,
                    LengthMultiplier = 1
                };

                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, position, color, 90, 1.5f, state);
            }
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
                if (entity is Bullet)
                {
                    entity.Velocity += (entity.Position - Position).ScaleTo(0.3f);
                }
                else
                {
                    var diference = Position - entity.Position;
                    var length = diference.Length();

                    entity.Velocity += diference.ScaleTo(MathHelper.Lerp(2, 0, length / 250f));
                }
            }

            if ((gameTime.TotalGameTime.Milliseconds / 250) % 2 == 0)
                SprayOrbitingParticles();
        }

        private void SprayOrbitingParticles()
        {
            var velocity = MathUtil.FromPolar(sprayAngle, Random.NextFloat(12, 15));
            var color = ColorUtil.HsvToColor(5, 0.5f, 0.8f);
            var position = Position + 2f * new Vector2(velocity.Y, -velocity.X) + Random.NextVector(4, 8);
            var state = new ParticleState
            {
                Velocity = velocity,
                LengthMultiplier = 1,
                Type = ParticleType.Enemy
            };

            GameRoot.ParticleManager.CreateParticle(Art.LineParticle, position, color, 190, 1.5f, state);

            sprayAngle -= MathHelper.TwoPi / 50f;
        }
    }
}