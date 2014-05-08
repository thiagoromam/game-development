using Funq;
using MapEditor.Editor;
using MapEditor.Input;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Input;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Ioc
{
    public static class App
    {
        public static readonly Container Container;
        private static Settings _settings;
        private static MouseControl _mouseControl;
        private static GuiText _guiText;

        static App()
        {
            Container = new Container();
        }

        public static void Register(SpriteBatch spriteBatch)
        {
            Container.Register(c => (IReadableSettings)_settings);
            Container.Register(c => (ISettings)_settings);
            Container.Register(c => (IMouseInput)_mouseControl);
            Container.Register(c => (IMouseComponent)_mouseControl);
            Container.Register(c => (IGuiText)_guiText);

            _settings = new Settings();
            _mouseControl = new MouseControl();
            _guiText = new GuiText(spriteBatch);
        }
    }
}