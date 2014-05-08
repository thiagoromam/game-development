using MapEditor.Helpers;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public class Text
    {
        private readonly SpriteFont _font;
        private readonly SpriteBatch _spriteBatch;
        private readonly IMouseInput _mouseInput;

        public Text(SpriteFont font, SpriteBatch spriteBatch)
        {
            Size = 1;
            Color = Color.White;
            _font = font;
            _spriteBatch = spriteBatch;
            _mouseInput = App.Container.Resolve<IMouseInput>();
        }

        public float Size { get; set; }
        public Color Color { get; set; }

        public bool Draw(string text, float x, float y, bool clickable)
        {
            var position = new Vector2(x, y);

            if (clickable)
                return DrawClickable(text, position);
            
            Draw(text, position);
            return false;
        }

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

            if (_font.Intersects(text, textPosition, Size, _mouseInput.Position))
            {
                Color = Color.Yellow;
                if (_mouseInput.LeftButtonClick)
                    clicked = true;
            }

            Draw(text, textPosition);

            return clicked;
        }
    }
}