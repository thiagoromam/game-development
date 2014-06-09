using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieSmashers.Particles
{
    public class Fire : Particle
    {
        public Fire(Vector2 loc, Vector2 traj, float size, int icon)
        {
            Location = loc;
            Trajectory = traj;
            Size = size;
            Flag = icon;
            Exists = true;
            Frame = 0.5f;
            Additive = true;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D spritesTex)
        {
            if (Frame > 0.5f)
                return;

            var sRect = new Rectangle(Flag * 64, 64, 64, 64);
            float tSize;

            if (Frame > 0.4f)
            {
                R = 1;
                G = 1;
                B = (Frame - 0.4f) * 10;
                tSize = Frame > 0.45f ? (0.5f - Frame) * Size * 20f : Size;
            }
            else if (Frame > 0.3f)
            {
                R = 1;
                G = (Frame - 0.3f) * 10;
                B = 0;
                tSize = Size;
            }
            else
            {
                R = Frame * 3.3f;
                G = 0;
                B = 0;
                tSize = (Frame / 0.3f) * Size;
            }

            Rotation = Flag % 2 == 0 ? Frame * 7 + Size * 20 : -Frame * 11 + Size * 20;

            var color = new Color(new Vector4(R, G, B, 1));
            var origin = new Vector2(32);
            spriteBatch.Draw(spritesTex, GameLocation, sRect, color, Rotation, origin, tSize, SpriteEffects.None, 1);
        }
    }
}