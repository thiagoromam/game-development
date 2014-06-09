using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class Smoke : Particle
    {
        public Smoke(Vector2 location, Vector2 trajectory, float r, float g, float b, float a, float size, int icon)
        {
            Location = location;
            Trajectory = trajectory;
            R = r;
            G = g;
            B = b;
            A = a;
            Size = size;
            Flag = icon;
            Owner = -1;
            Exists = true;
            Frame = 1;
        }

        public override void Update(float gameTime, Map map, ParticlesManager pMan, Character[] c)
        {
            if (Frame < 0.5f)
            {
                if (Trajectory.Y < -10) Trajectory.Y += gameTime * 500;
                if (Trajectory.X < -10) Trajectory.X += gameTime * 150;
                if (Trajectory.X > 10) Trajectory.X -= gameTime * 150;
            }

            base.Update(gameTime, map, pMan, c);
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D spritesTex)
        {
            var sRect = new Rectangle(Flag * 64, 0, 64, 64);
            var frameAlpha = Frame > 0.9f ? (1 - Frame) * 10 : Frame / 0.9f;
            var color = new Color(new Vector4(Frame * R, Frame * G, Frame * B, frameAlpha * A));
            var scale = Size + (1 - Frame);

            spriteBatch.Draw(spritesTex, GameLocation, sRect, color, Rotation, new Vector2(32), scale, SpriteEffects.None, 1);
        }
    }
}