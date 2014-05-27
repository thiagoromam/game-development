using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public interface IKeyFramesScroll : IScroll
    {
        bool IsCurrentKeyFrameVisible();
    }
    
    public class KeyFramesScroll : Scroll, IKeyFramesScroll
    {
        private readonly IReadOnlySettings _settings;

        public KeyFramesScroll(int limit, int x, int y)
            : base(Animation.KeyFramesCount, limit, x, y, y + 170)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
        }

        public bool IsCurrentKeyFrameVisible()
        {
            return IsIndexVisible(_settings.SelectedKeyFrameIndex);
        }
    }
}