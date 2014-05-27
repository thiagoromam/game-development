using System.Linq;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class AddFrameButton : TextButton
    {
        private readonly IFrameSelector _frameSelector;
        private readonly IFramesScroll _framesScroll;
        private readonly IReadOnlySettings _settings;

        public AddFrameButton(IFrameSelector frameSelector, IFramesScroll framesScroll, int x, int y)
            : base("(a)", x, y)
        {
            _frameSelector = frameSelector;
            _framesScroll = framesScroll;
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();

            UpdateFocus();
            _settings.SelectedFrameChanged += UpdateFocus;
            _framesScroll.ScrollIndexChanged += UpdateVisibility;
            _framesScroll.ScrollIndexChanged += UpdatePosition;
            Click = AddFrame;
        }

        private void AddFrame()
        {
            var keyFrame = _settings.SelectedAnimation.KeyFrames.FirstOrDefault(k => k.FrameReference == -1);
            if (keyFrame == null)
                return;

            keyFrame.Duration = 1;
            keyFrame.FrameReference = _settings.SelectedFrameIndex;
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