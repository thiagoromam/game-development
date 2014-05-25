using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;

namespace CharacterEditor.Editor.Controls.Frame
{
    public class FrameNameEditor : TextEditor
    {
        private readonly FrameSelector _frameSelector;
        private readonly ISettings _settings;

        public FrameNameEditor(FrameSelector frameSelector, int x, int y)
            : base(x, y)
        {
            _frameSelector = frameSelector;
            _settings = DependencyInjection.Resolve<ISettings>();

            _settings.SelectedFrameChanged += UpdateText;
            Change = v => _settings.SelectedFrame.Name = v;
            UpdateText();
        }

        private void UpdateText()
        {
            Text = _settings.SelectedFrame.Name.HasValue() ? _settings.SelectedFrame.Name : "<name>";
            Position.Y = _frameSelector.SelectedOption.Position.Y;
        }
    }
}