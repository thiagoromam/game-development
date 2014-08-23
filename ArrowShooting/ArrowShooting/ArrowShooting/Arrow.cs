using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArrowShooting
{
    public class Arrow
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _origin;
        private Vector2 _position;
        private Vector2 _direction;
        private float _gravity = 32;

        public Arrow(Vector2 position, float angle, float speed, Texture2D texture)
        {
            _position = position;
            _texture = texture;
            _direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
            _origin = new Vector2(texture.Width, texture.Height) / 2;
        }

        public void Update(float frameTime)
        {
            _gravity += _gravity * frameTime;
            _direction.Y += _gravity * frameTime;
            _position += _direction * frameTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var angle = (float)Math.Atan2(_direction.Y, _direction.X);

            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                angle,
                _origin,
                0.1f,
                SpriteEffects.None,
                1
            );
        }
    }
}