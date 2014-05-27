using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class AddFrameButton : TextButton
    {
        private readonly IFrameSelector _frameSelector;
        private readonly IFramesScroll _framesScroll;

        public AddFrameButton(IFrameSelector frameSelector, IFramesScroll framesScroll, int x, int y)
            : base("(a)", x, y)
        {
            _frameSelector = frameSelector;
            _framesScroll = framesScroll;
            var settings = DependencyInjection.Resolve<IReadonlySettings>();

            UpdateFocus();
            settings.SelectedFrameChanged += UpdateFocus;
            _framesScroll.ScrollIndexChanged += UpdateVisibility;
            _framesScroll.ScrollIndexChanged += UpdatePosition;
        }

        private void UpdateVisibility()
        {
            Visible = _framesScroll.IsCurrentFrameVisible();
        }

        private void UpdatePosition()
        {
            if (Visible)
                Position.Y = _frameSelector.SelectedOption.Position.Y;
        }

        private void UpdateFocus()
        {
            Visible = true;
            UpdatePosition();
        }
    }
}