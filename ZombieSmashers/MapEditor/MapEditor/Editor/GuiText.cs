using MapEditor.Helpers;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Editor
{
    public class GuiText : IGuiText
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly float _size;
        private readonly IMouseInput _mouseInput;

        public GuiText(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            Font = Art.Arial;
            _size = 0.8f;
            _mouseInput = App.Container.Resolve<IMouseInput>();
        }

        public SpriteFont Font { get; private set; }

        public bool MouseIntersects(string text, Vector2 position)
        {
            return Font.Intersects(text, position, _size, _mouseInput.Position);
        }

        public void Draw(string text, Vector2 position)
        {
            Draw(text, position, Color.White);
        }

        public void Draw(string text, Vector2 position, Color color)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Font, text, position, color, 0, Vector2.Zero, _size, SpriteEffects.None, 1);
            _spriteBatch.End();
        }
    }
}
