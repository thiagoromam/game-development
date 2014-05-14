using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Ioc
{
    public static class App
    {
        public static void Register(SpriteBatch spriteBatch)
        {
            TextLib.App.Register(spriteBatch);
        }
    }
}