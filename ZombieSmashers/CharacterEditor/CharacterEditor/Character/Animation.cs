using Helpers;

namespace CharacterEditor.Character
{
    public class Animation
    {
        public string Name;

        public Animation()
        {
            Name = string.Empty;
            KeyFrames = EnumerableHelper.Array<KeyFrame>(64);
        }

        public KeyFrame[] KeyFrames { get; private set; }
    }
}