using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Particles
{
    public class Particle
    {
        public Vector2 Location;
        public Vector2 Trajectory;
        protected float Frame;
        protected float R, G, B, A;
        protected float Size;
        protected float Rotation;

        protected int Flag;
        public int Owner;

        public bool Exists;
        public bool Background;
        public bool Additive;

        public Vector2 GameLocation
        {
            get { return Location - Game1.Scroll; }
        }

        public virtual void Update(float gameTime, Map map, ParticleManager pMan, Character[] c)
        {
            Location += Trajectory*gameTime;
            Frame -= gameTime;
            if (Frame < 0)
                KillMe();
        }

        private void KillMe()
        {
            Exists = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D spritesTex)
        {
            
        }
    }
}