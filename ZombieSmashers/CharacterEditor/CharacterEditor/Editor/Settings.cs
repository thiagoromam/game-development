using System;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;

namespace CharacterEditor.Editor
{
    public class Settings : ISettings, IReadonlySettings
    {
        private readonly CharacterDefinition _characterDefinition;
        private int _selectedFrameIndex;
        private int _selectedPartIndex;

        public Settings()
        {
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            SelectedFrame = _characterDefinition.Frames[SelectedFrameIndex];
            SelectedPart = SelectedFrame.Parts[SelectedPartIndex];
        }

        public int SelectedFrameIndex
        {
            get { return _selectedFrameIndex; }
            set
            {
                if (value == _selectedFrameIndex)
                    return;
                
                _selectedFrameIndex = value;
                SelectedFrame = _characterDefinition.Frames[SelectedFrameIndex];
                SelectedPartIndex = 0;
                OnSelectedFrameChanged();
            }
        }
        public int SelectedPartIndex
        {
            get { return _selectedPartIndex; }
            set
            {
                if (value == SelectedPartIndex)
                    return;

                _selectedPartIndex = value;
                SelectedPart = SelectedFrame.Parts[SelectedPartIndex];
                OnSelectedPartChanged();
            }
        }

        public Frame SelectedFrame { get; private set; }
        public Part SelectedPart { get; private set; }

        public event Action SelectedFrameChanged;
        public event Action SelectedPartChanged;

        protected virtual void OnSelectedFrameChanged()
        {
            var handler = SelectedFrameChanged;
            if (handler != null) handler();
        }
        protected virtual void OnSelectedPartChanged()
        {
            var handler = SelectedPartChanged;
            if (handler != null) handler();
        }
    }
}