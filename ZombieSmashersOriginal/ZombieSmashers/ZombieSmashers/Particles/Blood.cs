using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class Blood : Particle
    {
        public Blood(Vector2 loc, Vector2 traj, float r, float g, float b, float a, float size, int icon)
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
            Rotation = GlobalFunctions.GetAngle(Vector2.Zero, traj);
            Frame = Rand.GetRandomFloat(0.3f, 0.7f);
        }

        public override void Update(float gameTime, Map map, ParticleManager pMan, Character[] c)
        {
            Trajectory.Y += gameTime * 100f;

            if (Trajectory.X < -10f) Trajectory.X += gameTime * 200f;
            if (Trajectory.X > 10f) Trajectory.X -= gameTime * 200f;

            Rotation = GlobalFunctions.GetAngle(Vector2.Zero, Trajectory);

            base.Update(gameTime, map, pMan, c);
        }

        public override void Draw(SpriteBatch sprite, Texture2D spritesTex)
        {
            var sRect = new Rectangle(Flag * 64, 0, 64, 64);
            float frameAlpha;
            if (Frame > 0.9f) frameAlpha = (1.0f - Frame) * 10f;
            else frameAlpha = (Frame / 0.9f);
            sprite.Draw(spritesTex, GameLocation, sRect, new Color(new Vector4(R, G, B, A * frameAlpha)), Rotation,
                new Vector2(32f, 32f), new Vector2(Size * 2f, Size * 0.5f), SpriteEffects.None, 1.0f);
        }
    }
}