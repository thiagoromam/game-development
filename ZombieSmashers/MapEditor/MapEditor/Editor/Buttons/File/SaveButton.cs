using System.IO;
using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor.Buttons.File
{
    public class SaveButton : Button
    {
        private readonly IReadableSettings _settings;

        public SaveButton(int x, int y) : base(3, x, y)
        {
            _settings = App.Container.Resolve<IReadableSettings>();
        }

        public override void Update()
        {
            base.Update();

            if (Clicked)
                Save();
        }

        private void Save()
        {
        }
    }
}
