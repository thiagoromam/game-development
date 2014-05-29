using CharacterEditor.Ioc.Api.Character;
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
            var definitionsLoader = DependencyInjection.Resolve<IDefinitionsLoader>();

            UpdateFocus();
            _settings.SelectedAnimationChanged += UpdateFocus;
            _animationsScroll.ScrollIndexChanged += UpdateVisibility;
            _animationsScroll.ScrollIndexChanged += UpdatePosition;
            definitionsLoader.DefinitionsLoaded += UpdateText;
            Change = v => _settings.SelectedAnimation.Name = v;
        }

        private void UpdatePosition()
        {
            if (Visible)
                Position.Y = _animationSelector.SelectedOption.Position.Y;
        }

        private void UpdateVisibility()
        {
            Visible = _animationsScroll.IsCurrentAnimationVisible();
        }

        private void UpdateFocus()
        {
            Visible = true;
            UpdateText();
            UpdatePosition();
        }

        private void UpdateText()
        {
            Text = _settings.SelectedAnimation.Name.HasValue() ? _settings.SelectedAnimation.Name : "<name>";
        }
    }
}