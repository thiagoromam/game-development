using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter.ParticleSystem
{
    public class Particle<T>
    {
        private Texture2D _image;

        public Vector2 Position;
        public float Orientation;
        public Vector2 Scale;
        public Color Color;
        public float Duration;
        public float LifePercent;
        public T State;

        public Texture2D Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Origin = new Vector2(_image.Width / 2f, _image.Height / 2f);
            }
        }
        public Vector2 Origin { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color, Orientation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}