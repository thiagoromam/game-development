using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Controls.File
{
    public class MapPathEditor : TextEditor
    {
        public MapPathEditor(int x, int y) : base(x, y)
        {
            var settings = DependencyInjection.Resolve<ISettings>();

            Text = settings.MapPath;
            Change = v => settings.MapPath = v;
        }
    }
}