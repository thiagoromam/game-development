using System;
using Helpers;

namespace CharacterEditor.Character
{
    public class KeyFrame
    {
        public event Action FrameReferenceChanged;
        public int Duration;
        private int _frameReference;

        public KeyFrame()
        {
            FrameReference = -1;
            Scripts = EnumerableHelper.Array(4, string.Empty);
        }

        public int FrameReference
        {
            get { return _frameReference; }
            set
            {
                if (value == _frameReference)
                    return;
                
                _frameReference = value;
                OnFrameReferenceChanged();
            }
        }

        public string[] Scripts { get; private set; }

        private void OnFrameReferenceChanged()
        {
            var handler = FrameReferenceChanged;
            if (handler != null) handler();
        }
    }
}