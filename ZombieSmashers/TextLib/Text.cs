using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using TextLib.Api;

namespace TextLib
{
    internal class Text : IText, ITextContent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly IMouseInput _mouseInput;

        public Text(SpriteBatch spriteBatch, IMouseInput mouseInput)
        {
            _spriteBatch = spriteBatch;
            _mouseInput = mouseInput;
        }

        public SpriteFont Font { get; set; }
        public float Size { get; set; }

        public bool MouseIntersects(string text, Vector2 position)
        {
            return Font.Intersects(text, position, Size, _mouseInput.Position);
        }

        public void Draw(string text, Vector2 position)
        {
            Draw(text, position, Color.White);
        }

        public void Draw(string text, Vector2 position, Color color)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Font, text, position, color, 0, Vector2.Zero, Size, SpriteEffects.None, 1);
            _spriteBatch.End();
        }
    }
}
