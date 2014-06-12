using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class Hit : Particle
    {
        public Hit(Vector2 loc, Vector2 traj, int owner, int flag)
        {
            Location = loc;
            Trajectory = traj;
            Owner = owner;
            Flag = flag;
            Exists = true;
            Frame = 0.5f;
        }

        public override void Update(float gameTime, Map map, ParticleManager pMan, Character[] c)
        {
            HitManager.CheckHit(this, c, pMan);
            KillMe();
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D spritesTex)
        {
        }
    }
}