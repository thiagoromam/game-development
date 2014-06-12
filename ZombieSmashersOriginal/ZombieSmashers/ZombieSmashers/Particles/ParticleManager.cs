using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class ParticleManager
    {
        private readonly Particle[] _particles;
        private readonly SpriteBatch _spriteBatch;

        public ParticleManager(SpriteBatch spriteBatch)
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

        public void MakeBullet(Vector2 loc, Vector2 traj, CharDir face, int owner)
        {
            switch (face)
            {
                case CharDir.Left:
                    AddParticle(new Bullet(loc,
                        new Vector2(-traj.X, traj.Y) + Rand.GetRandomVector2(-90f, 90f, -90f, 90f), owner));
                    MakeMuzzleFlash(loc, new Vector2(-traj.X, traj.Y));
                    break;
                case CharDir.Right:
                    AddParticle(new Bullet(loc, traj + Rand.GetRandomVector2(-90f, 90f, -90f, 90f), owner));
                    MakeMuzzleFlash(loc, traj);
                    break;
            }
        }

        private void MakeMuzzleFlash(Vector2 loc, Vector2 traj)
        {
            for (var i = 0; i < 16; i++)
            {
                AddParticle(new MuzzleFlash(loc + (traj * i) * 0.001f + Rand.GetRandomVector2(-5f, 5f, -5f, 5f),
                    traj / 5f, (20f - i) * 0.06f));
            }
            for (var i = 0; i < 4; i++)
            {
                AddParticle(new Smoke(loc, Rand.GetRandomVector2(-30f, 30f, -100f, 0f), 0f, 0f, 0f, 0.25f,
                    Rand.GetRandomFloat(0.25f, 1.0f), Rand.GetRandomInt(0, 4)));
            }
        }

        public void MakeBulletDust(Vector2 loc, Vector2 traj)
        {
            for (var i = 0; i < 16; i++)
            {
                AddParticle(new Smoke(loc,
                    Rand.GetRandomVector2(-50f, 50f, -50f, 10f) - traj * Rand.GetRandomFloat(0.001f, 0.1f), 1f, 1f, 1f,
                    0.25f, Rand.GetRandomFloat(0.05f, 0.25f), Rand.GetRandomInt(0, 4)));
                AddParticle(new Smoke(loc, Rand.GetRandomVector2(-50f, 50f, -50f, 10f), 0.5f, 0.5f, 0.5f, 0.25f,
                    Rand.GetRandomFloat(0.1f, 0.5f), Rand.GetRandomInt(0, 4)));
            }
        }

        public void MakeBulletBlood(Vector2 loc, Vector2 traj)
        {
            for (var t = 0; t < 32; t++)
            {
                AddParticle(new Blood(loc,
                    traj * -1f * Rand.GetRandomFloat(0.01f, 0.1f) + Rand.GetRandomVector2(-50f, 50f, -50f, 50f), 1f, 0f, 0f,
                    1f, Rand.GetRandomFloat(0.1f, 0.3f), Rand.GetRandomInt(0, 4)));
            }
        }
    }
}