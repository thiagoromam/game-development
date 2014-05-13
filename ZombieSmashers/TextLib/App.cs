using Funq.Fast;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using TextLib.Api;

namespace TextLib
{
    public static class App
    {
        private static bool _registered;
        
        public static void Register(SpriteBatch spriteBatch)
        {
            if (_registered) return;
            _registered = true;

            MouseLib.App.Register();
            
            var text = new Text(spriteBatch, DependencyInjection.Resolve<IMouseInput>());
            
            DependencyInjection.Register((IText)text);
            DependencyInjection.Register((ITextContent)text);
        }
    }
}
