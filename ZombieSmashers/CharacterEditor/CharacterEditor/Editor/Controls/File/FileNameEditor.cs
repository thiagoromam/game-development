using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.File
{
    public class FileNameEditor : TextEditor
    {
        public FileNameEditor() : base(270, 15)
        {
            var settings = DependencyInjection.Resolve<ISettings>();

            Text = settings.FileName;
            Change = v => settings.FileName = v;
        }
    }
}