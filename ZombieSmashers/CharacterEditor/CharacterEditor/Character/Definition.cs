using Helpers;

namespace CharacterEditor.Character
{
    public class CharacterDefinition
    {
        public const int AnimationsCount = 64;
        public const int FramesCount = 512;

        public string Path;
        public int HeadIndex;
        public int TorsoIndex;
        public int LegsIndex;
        public int WeaponIndex;

        public CharacterDefinition()
        {
            Path = "char";

            Animations = EnumerableHelper.Array<Animation>(AnimationsCount);
            Frames = EnumerableHelper.Array<Frame>(FramesCount);
        }

        public Animation[] Animations { get; private set; }
        public Frame[] Frames { get; private set; }
    }
}