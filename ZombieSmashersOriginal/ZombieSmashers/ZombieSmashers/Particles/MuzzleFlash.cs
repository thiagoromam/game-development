using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieSmashers.Particles
{
    public class MuzzleFlash : Particle
    {
        public MuzzleFlash(Vector2 loc, Vector2 traj, float size)
        {
            Location = loc;
            Trajectory = traj;
            Size = size;
            Rotation = Rand.GetRandomFloat(0f, 6.28f);
            Exists = true;
            Frame = 0.05f;
            Additive = true;
        }

        public override void Draw(SpriteBatch sprite, Texture2D spritesTex)
        {
            sprite.Draw(spritesTex, GameLocation, new Rectangle(64, 128, 64, 64),
                new Color(new Vector4(1f, 0.8f, 0.6f, Frame * 8f)), Rotation, new Vector2(32.0f, 32.0f), Size - Frame,
                SpriteEffects.None, 1.0f);
        }
    }
}