using System;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.Managers;

namespace NeonVectorShooter.ParticleSystem
{
    public struct ParticleState
    {
        public Vector2 Velocity;
        public ParticleType Type;
        public float LengthMultiplier;

        public ParticleState(Vector2 velocity, ParticleType type) : this()
        {
            Velocity = velocity;
            Type = type;
            LengthMultiplier = 1;
        }

        public static void UpdateParticle(Particle<ParticleState> particle)
        {
            var velocity = particle.State.Velocity;

            particle.Position += velocity;
            particle.Orientation = velocity.ToAngle();

            var speed = velocity.Length();
            var alpha = Math.Min(1, Math.Min(particle.LifePercent * 2, speed * 1f));
            alpha = (float)Math.Pow(alpha, 2);

            particle.Color.A = (byte)(255 * alpha);
            particle.Scale.X = particle.State.LengthMultiplier * Math.Min(Math.Min(1, 0.2f * speed + 0.1f), alpha);

            if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.00000000001f)
                velocity = Vector2.Zero;

            if (particle.Position.X < 0)
                velocity.X = Math.Abs(velocity.X);
            else if (particle.Position.X > GameRoot.ScreenSize.X)
                velocity.X = -Math.Abs(velocity.X);
            
            if (particle.Position.Y < 0)
                velocity.Y = Math.Abs(velocity.Y);
            else if (particle.Position.Y > GameRoot.ScreenSize.Y)
                velocity.Y = -Math.Abs(velocity.Y);

            if (particle.State.Type != ParticleType.IgnoreGravity)
            {
                foreach (var blackHole in EntityManager.BlackHoles)
                {
                    var diference = blackHole.Position - particle.Position;
                    var distance = diference.Length();

                    var n = diference/distance;
                    velocity += 10000* n / (distance * distance + 10000);

                    if (distance < 400)
                        velocity += 45 * new Vector2(n.Y, -n.X) / (distance + 100);
                }
            }
        
            velocity *= 0.97f;
            particle.State.Velocity = velocity;

        }
    }
}