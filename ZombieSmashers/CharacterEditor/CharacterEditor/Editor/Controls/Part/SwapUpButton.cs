using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.Part
{
    public class SwapUpButton : IconButton
    {
        private readonly ISettings _settings;
        private readonly CharacterDefinition _characterDefinition;

        public SwapUpButton(int x, int y) : base(1, x, y)
        {
            _settings = DependencyInjection.Resolve<ISettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();

            Click = Swap;
        }

        private void Swap()
        {
            var frame = _characterDefinition.Frames[_settings.SelectedFrameIndex];

            if (_settings.SelectedPartIndex == 0)
                return;

            var aux = frame.Parts[_settings.SelectedPartIndex];
            frame.Parts[_settings.SelectedPartIndex] = frame.Parts[_settings.SelectedPartIndex - 1];
            frame.Parts[_settings.SelectedPartIndex - 1] = aux;
            _settings.SelectedPartIndex--;
            frame.NotifyPartsChanged();
        }
    }
}