using System;
using Helpers;

namespace CharacterEditor.Character
{
    public class Animation
    {
        public const int KeyFramesCount = 64;
        public event Action KeyFramesChanged;
        public string Name;

        public Animation()
        {
            Name = string.Empty;
            KeyFrames = EnumerableHelper.Array<KeyFrame>(KeyFramesCount);
        }

        public KeyFrame[] KeyFrames { get; private set; }

        public void NotifyKeyFramesChanged()
        {
            var handler = KeyFramesChanged;
            if (handler != null) handler();
        }
    }
}