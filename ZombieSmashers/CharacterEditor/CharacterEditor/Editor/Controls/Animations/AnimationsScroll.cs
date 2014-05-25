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
        private readonly IReadonlySettings _settings;

        public AnimationsScroll(int limit, int x, int y)
            : base(DependencyInjection.Resolve<CharacterDefinition>().Animations.Length, limit, x, y, y + 195)
        {
            _settings = DependencyInjection.Resolve<IReadonlySettings>();
        }

        public bool IsCurrentAnimationVisible()
        {
            return IsIndexVisible(_settings.SelectedAnimationIndex);
        }
    }
}