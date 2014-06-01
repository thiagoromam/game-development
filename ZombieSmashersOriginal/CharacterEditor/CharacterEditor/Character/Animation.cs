using System;

namespace CharacterEditor.Character
{
    class Animation
    {
        public string Name;
        private readonly KeyFrame[] _keyFrames;

        public Animation()
        {
            Name = String.Empty;

            _keyFrames = new KeyFrame[64];
            for (var i = 0; i < _keyFrames.Length; i++)
                _keyFrames[i] = new KeyFrame();
        }

        public KeyFrame[] KeyFrames
        {
            get { return _keyFrames; }
        }
    }
}
