using MapEditor.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public class Text
    {
        private readonly SpriteFont _font;
        private readonly SpriteBatch _spriteBatch;

        public Text(SpriteFont font, SpriteBatch spriteBatch)
        {
            Size = 1;
            Color = Color.White;
            _font = font;
            _spriteBatch = spriteBatch;
        }

        public float Size { get; set; }
        public Color Color { get; set; }

        public void Draw(string text, Vector2 position)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, text, position, Color, 0, Vector2.Zero, Size, SpriteEffects.None, 1);
            _spriteBatch.End();
        }

        public bool DrawClickable(string text, Vector2 textPosition, Vector2 mousePosition, bool mouseClick)
        {
            Color = Color.White;

            var clicked = false;

            if (_font.Intersects(text, textPosition, Size, mousePosition))
            {
                Color = Color.Yellow;
                if (mouseClick)
                    clicked = true;
            }

            Draw(text, textPosition);

            return clicked;
        }
    }
}