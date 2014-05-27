using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.Animations
{
    public interface IAnimationsScroll : IScroll
    {
        bool IsCurrentAnimationVisible();
    }

    public class AnimationsScroll : Scroll, IAnimationsScroll
    {
        private readonly IReadOnlySettings _settings;

        public AnimationsScroll(int limit, int x, int y)
            : base(CharacterDefinition.AnimationsCount, limit, x, y, y + 195)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
        }

        public bool IsCurrentAnimationVisible()
        {
            return IsIndexVisible(_settings.SelectedAnimationIndex);
        }
    }
}