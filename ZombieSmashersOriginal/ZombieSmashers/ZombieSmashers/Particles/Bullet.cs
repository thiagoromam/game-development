using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class Bullet : Particle
    {
        public Bullet(Vector2 loc, Vector2 traj, int owner)
        {
            Location = loc;
            Trajectory = traj;
            Owner = owner;
            Rotation = GlobalFunctions.GetAngle(Vector2.Zero, traj);
            Exists = true;
            Frame = 0.5f;
            Additive = true;
        }

        public override void Update(float gameTime, Map map, ParticleManager pMan, Character[] c)
        {
            if (map.CheckParticleCol(Location))
            {
                Frame = 0f;
                pMan.MakeBulletDust(Location, Trajectory);
            }
            base.Update(gameTime, map, pMan, c);
        }
        
        public override void Draw(SpriteBatch sprite, Texture2D spritesTex)
        {
            sprite.Draw(spritesTex, GameLocation, new Rectangle(0, 128, 64, 64),
                new Color(new Vector4(1f, 0.8f, 0.6f, 0.2f)), Rotation, new Vector2(32.0f, 32.0f), new Vector2(1f, 0.1f),
                SpriteEffects.None, 1.0f);
        }
    }
}