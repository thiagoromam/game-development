using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class FrameNameEditor : TextEditor
    {
        private readonly IFrameSelector _frameSelector;
        private readonly IFramesScroll _framesScroll;
        private readonly ISettings _settings;

        public FrameNameEditor(IFrameSelector frameSelector, IFramesScroll framesScroll, int x, int y)
            : base(x, y)
        {
            _frameSelector = frameSelector;
            _framesScroll = framesScroll;
            _settings = DependencyInjection.Resolve<ISettings>();

            UpdateFocus();
            _settings.SelectedFrameChanged += UpdateFocus;
            _framesScroll.ScrollIndexChanged += UpdateVisibility;
            _framesScroll.ScrollIndexChanged += UpdatePosition;
            Change = v => _settings.SelectedFrame.Name = v;
        }

        private void UpdatePosition()
        {
            Position.Y = _frameSelector.SelectedOption.Position.Y;
        }

        private void UpdateVisibility()
        {
            Visible = _framesScroll.IsCurrentFrameVisible();
        }

        private void UpdateFocus()
        {
            Visible = true;
            Text = _settings.SelectedFrame.Name.HasValue() ? _settings.SelectedFrame.Name : "<name>";
            UpdatePosition();
        }
    }
}