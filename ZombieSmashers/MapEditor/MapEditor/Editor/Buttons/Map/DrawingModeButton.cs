using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Buttons.Map
{
    public class DrawingModeButton : FlipTextButton<DrawingMode>
    {
        private readonly ISettings _settings;

        public DrawingModeButton(int x, int y)
            : base(x, y)
        {
            _settings = App.Container.Resolve<ISettings>();

            AddOption(DrawingMode.SegmentSelection, "select");
            AddOption(DrawingMode.CollisionMap, "colision");
            AddOption(DrawingMode.Ledge, "ledge");

            Value = _settings.DrawingMode;
            Change = v => _settings.DrawingMode = v;
        }
    }
}