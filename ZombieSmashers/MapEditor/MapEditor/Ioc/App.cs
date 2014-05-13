using Funq.Fast;
using MapEditor.Editor;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using MapEditor.MapClasses;
using MapEditor.Routines.Load;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Ioc
{
    public static class App
    {
        public static void Register()
        {
            MouseLib.App.Register();
            KeyboardLib.App.Register();

            var map = new Map();
            var settings = new Settings();

            DependencyInjection.Register((IReadOnlySettings)settings);
            DependencyInjection.Register((ISettings)settings);
            DependencyInjection.Register((IMapData)map);
            DependencyInjection.Register((IReadonlyMapData)map);
            DependencyInjection.Register((IMapComponent)map);
        }
        
        public static void Register(SpriteBatch spriteBatch)
        {
            TextLib.App.Register(spriteBatch);
            GraphicalUserInterfaceLib.App.Register(spriteBatch);
            
            DependencyInjection.Register((ILedgesLoader)new LedgesLoader());
        }
    }
}