using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter.Entities
{
    public abstract class Entity
    {
        private Texture2D _image;
        public Vector2 Origin;
        protected Color Color = Color.White;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Orientation;
        public float Radius = 20;
        public float Scale = 1;
        public bool IsExpired;

        protected Texture2D Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Size = _image != null ? new Vector2(_image.Width, _image.Height) : Vector2.Zero;
                Origin = Size / 2f;
            }
        }
        public Vector2 Size { get; private set; }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color, Orientation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}