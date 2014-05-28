using Funq.Fast;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using SharedLib;
using TextLib.Api;

namespace CharacterEditor
{
    public static class LegacySuport
    {
        private static IText _text;
        private static IMouseInput _mouseInput;
        private static SpriteBatch _spriteBatch;

        public static void Load(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _text = DependencyInjection.Resolve<IText>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
        }

        public static bool DrawClickText(int x, int y, string s)
        {
            var position = new Vector2(x, y);

            if (_text.MouseIntersects(s, position))
            {
                _text.Draw(s, position, Color.Yellow);
                return _mouseInput.LeftButtonClick;
            }

            _text.Draw(s, position);

            return false;
        }
        public static bool DrawButton(int x, int y, int index)
        {
            var r = false;

            var sRect = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            var dRect = new Rectangle(x, y, 32, 32);

            if (dRect.Contains(_mouseInput.Position))
            {
                dRect.X -= 1;
                dRect.Y -= 1;
                dRect.Width += 2;
                dRect.Height += 2;

                if (_mouseInput.LeftButtonClick)
                    r = true;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Icons, dRect, sRect, Color.White);
            _spriteBatch.End();

            return r;
        }
    }
}