using Helpers;

namespace CharacterEditor.Character
{
    public class CharacterDefinition
    {
        public string Path;
        public int HeadIndex;
        public int TorsoIndex;
        public int LegsIndex;
        public int WeaponIndex;

        public CharacterDefinition()
        {
            Path = "char";

            Animations = EnumerableHelper.Array<Animation>(64);
            Frames = EnumerableHelper.Array<Frame>(512);
        }

        public Animation[] Animations { get; private set; }
        public Frame[] Frames { get; private set; }
    }
}