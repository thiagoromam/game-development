using System;
using Helpers;

namespace CharacterEditor.Character
{
    public class CharacterDefinition
    {
        public const int AnimationsCount = 64;
        public const int FramesCount = 512;

        public event Action AnimationsChanged;
        public event Action FramesChanged;

        public int HeadIndex;
        public int TorsoIndex;
        public int LegsIndex;
        public int WeaponIndex;

        public CharacterDefinition()
        {
            Animations = EnumerableHelper.Array<Animation>(AnimationsCount);
            Frames = EnumerableHelper.Array<Frame>(FramesCount);
        }

        public Animation[] Animations { get; private set; }
        public Frame[] Frames { get; private set; }

        public void OnAnimationsChanged()
        {
            var handler = AnimationsChanged;
            if (handler != null) handler();
        }
        public void OnFramesChanged()
        {
            var handler = FramesChanged;
            if (handler != null) handler();
        }
    }
}