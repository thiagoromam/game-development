using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.Frames
{
    public interface IFramesScroll : IScroll
    {
        bool IsCurrentFrameVisible();
    }

    public class FramesesScroll : Scroll, IFramesScroll, IControlComponent, IControl
    {
        private readonly IReadonlySettings _settings;

        public FramesesScroll(int limit, int x, int y)
            : base(DependencyInjection.Resolve<CharacterDefinition>().Frames.Length, limit, x, y, y + 290)
        {
            _settings = DependencyInjection.Resolve<IReadonlySettings>();
        }

        public bool IsCurrentFrameVisible()
        {
            return IsIndexVisible(_settings.SelectedFrameIndex);
        }
    }
}