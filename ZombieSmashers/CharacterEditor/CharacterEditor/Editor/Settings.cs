using System;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;

namespace CharacterEditor.Editor
{
    public sealed class Settings : ISettings, IReadOnlySettings
    {
        private readonly CharacterDefinition _characterDefinition;
        private int _selectedAnimationIndex;
        private int? _selectedKeyFrameIndex;
        private int _selectedFrameIndex;
        private int? _selectedPartIndex;

        public Settings()
        {
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            SelectedAnimation = _characterDefinition.Animations[SelectedAnimationIndex];
            SelectedFrame = _characterDefinition.Frames[SelectedFrameIndex];
            SelectedPart = SelectedFrame.Parts[SelectedPartIndex];
        }

        public int SelectedAnimationIndex
        {
            get { return _selectedAnimationIndex; }
            set
            {
                if (value == SelectedAnimationIndex)
                    return;

                var previousIndex = _selectedAnimationIndex;
                _selectedAnimationIndex = value;
                SelectedAnimation = _characterDefinition.Animations[value];
                OnSelectedAnimationIndexChanged(previousIndex, value);
                OnSelectedAnimationChanged();
                _selectedKeyFrameIndex = null;
                SelectedKeyFrameIndex = 0;
            }
        }
        public int SelectedKeyFrameIndex
        {
            get { return _selectedKeyFrameIndex ?? 0; }
            set
            {
                _selectedKeyFrameIndex = value;
                SelectedKeyFrame = SelectedAnimation.KeyFrames[value];
                OnSelectedKeyFrameChanged();
            }
        }
        public int SelectedFrameIndex
        {
            get { return _selectedFrameIndex; }
            set
            {
                if (value == _selectedFrameIndex)
                    return;

                var previousIndex = SelectedFrameIndex;
                _selectedFrameIndex = value;
                SelectedFrame = _characterDefinition.Frames[value];
                OnSelectedFrameIndexChanged(previousIndex, value);
                OnSelectedFrameChanged();
                _selectedPartIndex = null;
                SelectedPartIndex = 0;
            }
        }
        public int SelectedPartIndex
        {
            get
            {
                return _selectedPartIndex ?? 0;
            }
            set
            {
                if (value == _selectedPartIndex)
                    return;

                _selectedPartIndex = value;
                SelectedPart = SelectedFrame.Parts[value];
                OnSelectedPartChanged();
            }
        }

        public Animation SelectedAnimation { get; private set; }
        public KeyFrame SelectedKeyFrame { get; private set; }
        public Frame SelectedFrame { get; private set; }
        public Part SelectedPart { get; private set; }

        public event AnimationIndexChangedHandler SelectedAnimationIndexChanged;
        public event FrameIndexChangedHandler SelectedFrameIndexChanged;
        public event Action SelectedAnimationChanged;
        public event Action SelectedKeyFrameChanged;

        public event Action SelectedFrameChanged;
        public event Action SelectedPartChanged;

        private void OnSelectedAnimationIndexChanged(int previousIndex, int newIndex)
        {
            var handler = SelectedAnimationIndexChanged;
            if (handler != null) handler(previousIndex, newIndex);
        }
        private void OnSelectedKeyFrameChanged()
        {
            var handler = SelectedKeyFrameChanged;
            if (handler != null) handler();
        }
        private void OnSelectedFrameIndexChanged(int previousIndex, int newIndex)
        {
            var handler = SelectedFrameIndexChanged;
            if (handler != null) handler(previousIndex, newIndex);
        }
        private void OnSelectedAnimationChanged()
        {
            var handler = SelectedAnimationChanged;
            if (handler != null) handler();
        }
        private void OnSelectedFrameChanged()
        {
            var handler = SelectedFrameChanged;
            if (handler != null) handler();
        }
        private void OnSelectedPartChanged()
        {
            var handler = SelectedPartChanged;
            if (handler != null) handler();
        }
    }
}