using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieSmashers.Particles
{
    public class Fog : Particle
    {
        public Fog(Vector2 loc)
        {
            Location = loc;
            Trajectory = new Vector2(80f, -30f);
            Size = Rand.GetRandomFloat(6f, 8f);
            Flag = Rand.GetRandomInt(0, 4);
            Owner = -1;
            Exists = true;
            Frame = (float)Math.PI * 2f;
            Additive = true;
            Rotation = Rand.GetRandomFloat(0f, 6.28f);
        }

        public override void Draw(SpriteBatch sprite, Texture2D spritesTex)
        {
            sprite.Draw(spritesTex, GameLocation, new Rectangle(Flag * 64, 0, 64, 64),
                new Color(new Vector4(1f, 1f, 1f, (float)Math.Sin(Frame / 2f) * .2f)), Rotation + Frame / 4f,
                new Vector2(32.0f, 32.0f), Size, SpriteEffects.None, 1.0f);
        }
    }
}