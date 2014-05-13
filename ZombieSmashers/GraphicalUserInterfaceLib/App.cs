using Microsoft.Xna.Framework.Graphics;

namespace GraphicalUserInterfaceLib
{
    public static class App
    {
        private static bool _registered;

        public static void Register(SpriteBatch spriteBatch)
        {
            if (_registered) return;
            _registered = true;

            MouseLib.App.Register();
            TextLib.App.Register(spriteBatch);
            KeyboardLib.App.Register();
        }
    }
}