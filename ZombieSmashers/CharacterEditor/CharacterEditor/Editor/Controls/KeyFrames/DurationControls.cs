using CharacterEditor.Ioc.Api.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;

// ReSharper disable ForCanBeConvertedToForeach
namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public class DurationControls : IControlComponent, ITextControl
    {
        private readonly IKeyFramesScroll _keyFramesScroll;
        private readonly IReadOnlySettings _settings;
        private readonly DurationControl[] _durationControls;

        public DurationControls(int x, int y, int yIncrement, IKeyFramesScroll keyFramesScroll, IKeyFramesOrderer keyFramesOrderer)
        {
            _keyFramesScroll = keyFramesScroll;
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _durationControls = new DurationControl[_keyFramesScroll.Limit];
            var definitionsLoader = DependencyInjection.Resolve<IDefinitionsLoader>();

            for (var i = 0; i < _durationControls.Length; i++)
                _durationControls[i] = new DurationControl(x, y + (i * yIncrement), keyFramesOrderer);

            _settings.SelectedAnimationChanged += UpdateControls;
            _settings.SelectedAnimation.KeyFramesChanged += UpdateControls;
            _keyFramesScroll.ScrollIndexChanged += UpdateControls;
            definitionsLoader.DefinitionsLoaded += UpdateControls;
            UpdateControls();
        }

        private void UpdateControls()
        {
            for (var i = 0; i < _durationControls.Length; i++)
            {
                var keyFrame = _settings.SelectedAnimation.KeyFrames[_keyFramesScroll.ScrollIndex + i];
                _durationControls[i].KeyFrame = keyFrame;
            }
        }

        public void Update()
        {
            for (var i = 0; i < _durationControls.Length; i++)
                _durationControls[i].Update();
        }

        public void Draw()
        {
            for (var i = 0; i < _durationControls.Length; i++)
                _durationControls[i].Draw();
        }
    }
}
