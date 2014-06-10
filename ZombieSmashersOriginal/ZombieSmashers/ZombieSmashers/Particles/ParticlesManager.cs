using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class ParticlesManager
    {
        private readonly Particle[] _particles;
        private readonly SpriteBatch _spriteBatch;

        public ParticlesManager(SpriteBatch spriteBatch)
        {
            _particles = new Particle[1024];
            _spriteBatch = spriteBatch;
        }

        public void AddParticle(Particle newParticle)
        {
            AddParticle(newParticle, false);
        }

        public void AddParticle(Particle newParticle, bool background)
        {
            for (var i = 0; i < _particles.Length; i++)
            {
                if (_particles[i] != null) continue;

                newParticle.Background = background;
                _particles[i] = newParticle;

                break;
            }
        }

        public void UpdateParticles(float frameTime, Map map, Character[] c)
        {
            for (var i = 0; i < _particles.Length; i++)
            {
                var particle = _particles[i];
                if (particle == null) continue;
                
                particle.Update(frameTime, map, this, c);
                if (!particle.Exists)
                    _particles[i] = null;
            }
        }

        public void DrawParticles(Texture2D spritesTex, bool background)
        {
            _spriteBatch.Begin();
            foreach (var particle in _particles)
            {
                if (particle == null || particle.Additive) continue;
                if (particle.Background == background)
                    particle.Draw(_spriteBatch, spritesTex);
            }
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var particle in _particles)
            {
                if (particle == null || !particle.Additive) continue;
                if (particle.Background == background)
                    particle.Draw(_spriteBatch, spritesTex);
            }
            _spriteBatch.End();
        }
    }
}