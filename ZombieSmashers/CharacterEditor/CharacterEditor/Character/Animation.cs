using Helpers;

namespace CharacterEditor.Character
{
    public class Animation
    {
        public const int KeyFramesCount = 64;
        public string Name;

        public Animation()
        {
            Name = string.Empty;
            KeyFrames = EnumerableHelper.Array<KeyFrame>(KeyFramesCount);
        }

        public KeyFrame[] KeyFrames { get; private set; }
    }
}