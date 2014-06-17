using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieSmashers.Particles
{
    public class BloodDust : Particle
    {
        public BloodDust(Vector2 loc, Vector2 traj, float r, float g, float b, float a, float size, int icon)
        {
            Location = loc;
            Trajectory = traj;
            R = r;
            G = g;
            B = b;
            A = a;
            Size = size;
            Flag = icon;
            Owner = -1;
            Exists = true;
            Frame = 1.0f;
        }

        public override void Draw(SpriteBatch sprite, Texture2D spritesTex)
        {
            sprite.Draw(spritesTex, GameLocation, new Rectangle(Flag * 64, 0, 64, 64),
                new Color(new Vector4(R, G, B, A * Frame)), Rotation, new Vector2(32.0f, 32.0f), Size, SpriteEffects.None,
                1.0f);
        }
    }
}