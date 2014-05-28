using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public interface IKeyFramesOrderer
    {
        void NeedsOrdering();
    }

    public class KeyFramesOrderer : IKeyFramesOrderer
    {
        private readonly IKeyFramesScroll _keyFramesScroll;
        private readonly IReadOnlySettings _settings;
        private bool _needsOrdering;

        public KeyFramesOrderer(IKeyFramesScroll keyFramesScroll)
        {
            _keyFramesScroll = keyFramesScroll;
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
        }

        public void NeedsOrdering()
        {
            _needsOrdering = true;
        }

        public void Update()
        {
            if (!_needsOrdering)
                return;
            
            int i;
            for (i = _keyFramesScroll.ScrollIndex; i < Animation.KeyFramesCount - 1; i++)
            {
                var current = _settings.SelectedAnimation.KeyFrames[i];
                if (current.FrameReference > -1 && current.Duration > 0)
                    continue;
                
                var next = _settings.SelectedAnimation.KeyFrames[i + 1];

                current.FrameReference = next.FrameReference;
                current.Duration = next.Duration;
                next.FrameReference = -1;
                next.Duration = 0;

                if (current.FrameReference == -1)
                    break;
            }

            if (i == Animation.KeyFramesCount - 1)
            {
                var last = _settings.SelectedAnimation.KeyFrames[i];
                last.FrameReference = -1;
                last.Duration = 0;
            }

            _needsOrdering = false;
            _settings.SelectedAnimation.NotifyKeyFramesChanged();
        }
    }
}