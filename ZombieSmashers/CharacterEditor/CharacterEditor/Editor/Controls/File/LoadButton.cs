using System.IO;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;


namespace CharacterEditor.Editor.Controls.File
{
    public class LoadButton : SharedLib.Gui.Controls.File.LoadButton
    {
        private readonly IReadOnlySettings _settings;
        private readonly IDefinitionsLoader _definitionsLoader;

        public LoadButton()
            : base(230, 5)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _definitionsLoader = DependencyInjection.Resolve<IDefinitionsLoader>();
        }

        protected override void Load()
        {
            var file = new BinaryReader(System.IO.File.Open("Content/" + _settings.FileName + ".zmx", FileMode.Open, FileAccess.Read));

            _definitionsLoader.Load(file);
            
            file.Close();
        }
    }
}