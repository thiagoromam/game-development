using CharacterEditor.Character;
using CharacterEditor.Editor;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Ioc
{
    public static class App
    {
        public static void Register()
        {
            KeyboardLib.App.Register();
            MouseLib.App.Register();

            DependencyInjection.Register(new CharacterDefinition());
            var settings = new Settings();
            DependencyInjection.Register((ISettings)settings);
            DependencyInjection.Register((IReadOnlySettings)settings);
        }

        public static void Register(SpriteBatch spriteBatch)
        {
            TextLib.App.Register(spriteBatch);
            GraphicalUserInterfaceLib.App.Register(spriteBatch);
        }
    }
}