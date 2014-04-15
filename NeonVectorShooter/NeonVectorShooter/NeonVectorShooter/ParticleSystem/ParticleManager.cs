using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonVectorShooter.Helpers;

namespace NeonVectorShooter.ParticleSystem
{
    public class ParticleManager<T>
    {
        private readonly CircularArray<Particle<T>> _particles;
        private readonly Action<Particle<T>> _updateParticle;

        public ParticleManager(int capacity, Action<Particle<T>> updateParticle)
        {
            _particles = new CircularArray<Particle<T>>(capacity);
            _updateParticle = updateParticle;
        }

        public void CreateParticle(Texture2D image, Vector2 position, Color color, float duration, Vector2 scale, T state, float theta = 0)
        {
            Particle<T> particle;
            if (_particles.Count == _particles.Capacity)
            {
                particle = _particles[0];
                _particles.Start++;
            }
            else
            {
                particle = _particles[_particles.Count];
                _particles.Count++;
            }

            particle.Image = image;
            particle.Position = position;
            particle.Color = color;
            particle.Duration = duration;
            particle.LifePercent = 1;
            particle.Scale = scale;
            particle.Orientation = theta;
            particle.State = state;
        }
        public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, float scale, T state, float theta = 0)
        {
            CreateParticle(texture, position, tint, duration, new Vector2(scale), state, theta);
        }

        public void Update()
        {
            var removalCount = 0;

            for (var i = 0; i < _particles.Count; i++)
            {
                var particle = _particles[i];
                _updateParticle(particle);
                particle.LifePercent -= 1 / particle.Duration;

                Swap(i - removalCount, i);

                if (particle.LifePercent <= 0)
                    removalCount++;
            }

            _particles.Count -= removalCount;
        }

        private void Swap(int index1, int index2)
        {
            var temp = _particles[index1];
            _particles[index1] = _particles[index2];
            _particles[index2] = temp;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _particles.Count; ++i)
                _particles[i].Draw(spriteBatch);
        }
    }
}