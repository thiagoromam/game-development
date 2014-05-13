using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework;

namespace MapEditor.Editor.Controls.Map.Ledge
{
    public class LedgeSelector : TextButtonList<int>
    {
        public LedgeSelector(int x, int y, int yIncrement)
        {
            var settings = DependencyInjection.Resolve<ISettings>();
            var mapData = DependencyInjection.Resolve<IReadonlyMapData>();

            for (var i = 0; i < mapData.Ledges.Length; i++)
                AddOption(i, "ledge " + i, new Vector2(x, y + i * yIncrement));

            Value = settings.SelectedLedge;
            Change = v => settings.SelectedLedge = v;
        }
    }
}