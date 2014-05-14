using Helpers;

namespace CharacterEditor.Character
{
    public class KeyFrame
    {
        public int FrameReference;
        public int Duration;

        public KeyFrame()
        {
            FrameReference = -1;
            Scripts = EnumerableHelper.Array(4, string.Empty);
        }

        public string[] Scripts { get; private set; }
    }
}