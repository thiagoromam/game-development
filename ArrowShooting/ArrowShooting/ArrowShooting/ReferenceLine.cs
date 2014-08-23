using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArrowShooting
{
    public class ReferenceLine
    {
        private readonly MouseInput _mouse;
        private readonly Texture2D _pixelTexture;
        private readonly Texture2D _stubTexture;
        private bool _show;
        private Vector2 _initialPosition;
        private float _angle;
        private Vector2 _lineLength = new Vector2(0, 1);

        public ReferenceLine(MouseInput mouse, Texture2D pixelTexture, Texture2D stubTexture)
        {
            _mouse = mouse;
            _pixelTexture = pixelTexture;
            _stubTexture = stubTexture;
        }

        public void Update()
        {
            if (_mouse.LeftPressed)
            {
                if (_mouse.LeftDown)
                {
                    _show = true;
                    _initialPosition = _mouse.Position;
                }

                var direction = _mouse.Position - _initialPosition;
                _angle = (float)Math.Atan2(direction.Y, direction.X);
                _lineLength.X = direction.Length();
            }
            else if (_mouse.LeftUp)
            {
                _show = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_show)
            {
                spriteBatch.Draw(
                    _pixelTexture,
                    _initialPosition,
                    null,
                    Color.White,
                    _angle,
                    Vector2.Zero,
                    _lineLength,
                    SpriteEffects.None,
                    1
                );
                spriteBatch.Draw(
                    _stubTexture,
                    _mouse.Position,
                    null,
                    Color.White,
                    _angle,
                    new Vector2(11, 9.5f), 
                    0.8f,
                    SpriteEffects.None,
                    1
                );
            }
        }
    }
}