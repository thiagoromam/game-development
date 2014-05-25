using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework;

namespace MapEditor.Editor.Controls.Map.Ledge
{
    public class LedgeSelector : TextButtonList<int>
    {
        private readonly ISettings _settings;

        public LedgeSelector(int x, int y, int yIncrement)
        {
            _settings = DependencyInjection.Resolve<ISettings>();
            var mapData = DependencyInjection.Resolve<IReadonlyMapData>();

            for (var i = 0; i < mapData.Ledges.Length; i++)
                AddOption(i, "ledge " + i, new Vector2(x, y + i * yIncrement));

            SelectedValue = _settings.SelectedLedge;
            Change = ValueChange;
        }

        private void ValueChange(int? previousValue, int? newValue)
        {
            // ReSharper disable once PossibleInvalidOperationException
            _settings.SelectedLedge = newValue.Value;
        }
    }
}