using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Controls.Map
{
    public class DrawingModeButton : FlipTextButton<DrawingMode>
    {
        public DrawingModeButton(int x, int y)
            : base(x, y)
        {
            var settings = DependencyInjection.Resolve<ISettings>();

            AddOption(DrawingMode.SegmentSelection, "select");
            AddOption(DrawingMode.CollisionMap, "colision");
            AddOption(DrawingMode.Ledge, "ledge");

            Value = settings.CurrentDrawingMode;
            Change = v => settings.CurrentDrawingMode = v;
        }
    }
}