using System;
using Microsoft.Xna.Framework;
using NeonVectorShooter.Helpers;

namespace NeonVectorShooter.ParticleSystem
{
    public struct ParticleState
    {
        public Vector2 Velocity;
        public ParticleType Type;
        public float LengthMultiplier;

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

            velocity *= 0.97f;
            particle.State.Velocity = velocity;
        }
    }
}