using MapEditor.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public class Text
    {
        private readonly SpriteFont _font;
        private readonly SpriteBatch _spriteBatch;
        private readonly MouseControl _mouseControl;

        public Text(SpriteFont font, SpriteBatch spriteBatch, MouseControl mouseControl)
        {
            Size = 1;
            Color = Color.White;
            _font = font;
            _spriteBatch = spriteBatch;
            _mouseControl = mouseControl;
        }

        public float Size { get; set; }
        public Color Color { get; set; }

        public void Draw(string text, Vector2 position)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, text, position, Color, 0, Vector2.Zero, Size, SpriteEffects.None, 1);
            _spriteBatch.End();
        }

        public bool DrawClickable(string text, Vector2 textPosition)
        {
            Color = Color.White;

            var clicked = false;

            if (_font.Intersects(text, textPosition, Size, _mouseControl.Position))
            {
                Color = Color.Yellow;
                if (_mouseControl.LeftButtonClick)
                    clicked = true;
            }

            Draw(text, textPosition);

            return clicked;
        }
    }
}