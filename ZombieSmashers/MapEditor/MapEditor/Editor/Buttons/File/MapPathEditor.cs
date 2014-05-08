using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Buttons.File
{
    public class MapPathEditor : TextEditor
    {
        private readonly ISettings _settings;

        public MapPathEditor(int x, int y) : base(x, y)
        {
            _settings = App.Container.Resolve<ISettings>();

            Text = _settings.MapPath;
            Change = v => _settings.MapPath = v;
        }
    }
}