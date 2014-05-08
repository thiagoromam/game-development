using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Buttons.Map
{
    public class MapLayerButton : FlipTextButton<int>
    {
        private readonly ISettings _settings;

        public MapLayerButton(int x, int y)
            : base(x, y)
        {
            _settings = App.Container.Resolve<ISettings>();

            AddOption(0, "back");
            AddOption(1, "mid");
            AddOption(2, "fore");

            Value = _settings.MapLayer;
            Change = v => _settings.MapLayer = v;
        }
    }
}