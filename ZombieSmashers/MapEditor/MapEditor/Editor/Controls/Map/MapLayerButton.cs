using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Controls.Map
{
    public class MapLayerButton : FlipTextButton<int>
    {
        public MapLayerButton(int x, int y) : base(x, y)
        {
            var settings = DependencyInjection.Resolve<ISettings>();

            AddOption(0, "back");
            AddOption(1, "mid");
            AddOption(2, "fore");

            Value = settings.CurrentMapLayer;
            Change = v => settings.CurrentMapLayer = v;
        }
    }
}