using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class FrameNameEditor : TextEditor
    {
        private readonly IFrameSelector _frameSelector;
        private readonly IFrameScroll _frameScroll;
        private readonly ISettings _settings;

        public FrameNameEditor(IFrameSelector frameSelector, IFrameScroll frameScroll, int x, int y)
            : base(x, y)
        {
            _frameSelector = frameSelector;
            _frameScroll = frameScroll;
            _settings = DependencyInjection.Resolve<ISettings>();

            UpdateFocus();
            _settings.SelectedFrameChanged += UpdateFocus;
            _frameScroll.ScrollIndexChanged += UpdateVisibility;
            _frameScroll.ScrollIndexChanged += UpdatePosition;
            Change = v => _settings.SelectedFrame.Name = v;
        }

        private void UpdatePosition()
        {
            Position.Y = _frameSelector.SelectedOption.Position.Y;
        }

        private void UpdateVisibility()
        {
            Visible = _frameScroll.IsCurrentFrameVisible();
        }

        private void UpdateFocus()
        {
            Visible = true;
            Text = _settings.SelectedFrame.Name.HasValue() ? _settings.SelectedFrame.Name : "<name>";
            UpdatePosition();
        }
    }
}