using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
{
    public abstract class Entity
    {
        private Texture2D _image;
        private Vector2 _origin;
        protected Color Color = Color.White;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Orientation;
        public float Radius = 20;
        public bool IsExpired;

        protected Texture2D Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Size = _image != null ? new Vector2(_image.Width, _image.Height) : Vector2.Zero;
                _origin = Size / 2f;
            }
        }
        public Vector2 Size { get; private set; }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color, Orientation, _origin, 1, SpriteEffects.None, 0);
        }
    }
}