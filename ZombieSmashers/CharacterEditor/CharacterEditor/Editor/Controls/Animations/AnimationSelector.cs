using System.Globalization;
using System.Linq;
using System.Text;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Animations
{
    public interface IAnimationSelector
    {
        TextButtonList<int>.TextButtonOption SelectedOption { get; }
    }

    public class AnimationSelector : TextButtonList<int>, IAnimationSelector
    {
        private readonly IAnimationsScroll _animationsScroll;
        private readonly ISettings _settings;
        private readonly CharacterDefinition _characterDefinition;
        private readonly TextButtonOption[] _options;

        public AnimationSelector(int x, int y, int yIncrement, IAnimationsScroll animationsScroll)
        {
            _animationsScroll = animationsScroll;
            _settings = DependencyInjection.Resolve<ISettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _options = new TextButtonOption[_animationsScroll.Limit];
            var definitionsLoader = DependencyInjection.Resolve<IDefinitionsLoader>();

            for (var i = 0; i < _options.Length; i++)
                _options[i] = AddOption(i, GetAnimationText(i), new Vector2(x, y + i * yIncrement));

            SelectedValue = _settings.SelectedAnimationIndex;
            Change = ValueChange;
            _animationsScroll.ScrollIndexChanged += UpdateOptions;
            _settings.SelectedAnimationIndexChanged += UpdateAnimationWithoutName;
            definitionsLoader.DefinitionsLoaded += UpdateOptions;
        }

        private void UpdateAnimationWithoutName(int previousIndex, int newIndex)
        {
            var previousAnimation = _characterDefinition.Animations[previousIndex];
            if (previousAnimation.Name.HasValue()) return;

            previousAnimation.Name = "animation" + previousIndex;

            if (_animationsScroll.IsIndexVisible(previousIndex))
                _options.Single(o => o.Value == previousIndex).Text = GetAnimationText(previousIndex);
        }

        private void UpdateOptions()
        {
            var isCurrentAnimationVisible = _animationsScroll.IsCurrentAnimationVisible();
            if (!isCurrentAnimationVisible)
                ClearValue();

            for (var i = 0; i < _options.Length; i++)
            {
                var option = _options[i];
                option.Value = _animationsScroll.ScrollIndex + i;
                option.Text = GetAnimationText(option.Value);
            }

            if (isCurrentAnimationVisible)
                SelectedValue = _settings.SelectedAnimationIndex;
        }

        private void ValueChange(int? previousValue, int? newValue)
        {
            if (newValue.HasValue)
            {
                _settings.SelectedAnimationIndex = newValue.Value;
                _options.Single(o => o.Value == newValue.Value).Text = GetAnimationText(newValue.Value);
            }

            if (previousValue.HasValue)
                _options.Single(o => o.Value == previousValue.Value).Text = GetAnimationText(previousValue.Value);
        }

        private string GetAnimationText(int animationIndex)
        {
            var text = new StringBuilder();
            text.Append(animationIndex.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            text.Append(":");

            if (animationIndex != _settings.SelectedAnimationIndex)
            {
                var animation = _characterDefinition.Animations[animationIndex];
                text.AppendFormat(" {0}", animation.Name);
            }

            return text.ToString();
        }
    }
}