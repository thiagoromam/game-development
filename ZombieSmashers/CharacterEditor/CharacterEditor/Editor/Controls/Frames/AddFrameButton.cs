using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class AddFrameButton : TextButton
    {
        private readonly IFrameSelector _frameSelector;
        private readonly IFrameScroll _frameScroll;

        public AddFrameButton(IFrameSelector frameSelector, IFrameScroll frameScroll, int x, int y) : base("(a)", x, y)
        {
            _frameSelector = frameSelector;
            _frameScroll = frameScroll;
            var settings = DependencyInjection.Resolve<IReadonlySettings>();

            UpdateFocus();
            settings.SelectedFrameChanged += UpdateFocus;
            _frameScroll.ScrollIndexChanged += UpdateVisibility;
            _frameScroll.ScrollIndexChanged += UpdatePosition;
        }

        private void UpdateVisibility()
        {
            Visible = _frameScroll.IsCurrentFrameVisible();
        }

        private void UpdatePosition()
        {
            Position.Y = _frameSelector.SelectedOption.Position.Y;
        }

        private void UpdateFocus()
        {
            Visible = true;
            UpdatePosition();
        }
    }
}