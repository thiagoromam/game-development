using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework;

namespace MapEditor.Editor.Controls.Map.Ledge
{
    public class LedgeSelector : RadioButtonList<int>
    {
        private readonly ISettings _settings;

        public LedgeSelector(int x, int y, int yIncrement, IReadonlyMapData mapData)
        {
            _settings = App.Container.Resolve<ISettings>();
            
            for (var i = 0; i < mapData.Ledges.Length; i++)
                AddOption(i, "ledge " + i, new Vector2(x, y + i * yIncrement));

            Value = _settings.SelectedLedge;
            Change = v => _settings.SelectedLedge = v;
        }
    }
}