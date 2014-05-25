using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;

namespace CharacterEditor.Editor.Controls.Animations
{
    public class AnimationNameEditor : TextEditor
    {
        private readonly IAnimationSelector _animationSelector;
        private readonly IAnimationsScroll _animationsScroll;
        private readonly ISettings _settings;

        public AnimationNameEditor(IAnimationSelector animationSelector, IAnimationsScroll animationsScroll, int x, int y)
            : base(x, y)
        {
            _animationSelector = animationSelector;
            _animationsScroll = animationsScroll;
            _settings = DependencyInjection.Resolve<ISettings>();

            UpdateFocus();
            _settings.SelectedAnimationChanged += UpdateFocus;
            _animationsScroll.ScrollIndexChanged += UpdateVisibility;
            _animationsScroll.ScrollIndexChanged += UpdatePosition;
            Change = v => _settings.SelectedAnimation.Name = v;
        }

        private void UpdatePosition()
        {
            Position.Y = _animationSelector.SelectedOption.Position.Y;
        }

        private void UpdateVisibility()
        {
            Visible = _animationsScroll.IsCurrentAnimationVisible();
        }

        private void UpdateFocus()
        {
            Visible = true;
            Text = _settings.SelectedAnimation.Name.HasValue() ? _settings.SelectedAnimation.Name : "<name>";
            UpdatePosition();
        }
    }
}