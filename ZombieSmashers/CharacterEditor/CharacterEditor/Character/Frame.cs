using Helpers;

namespace CharacterEditor.Character
{
    public class Frame
    {
        public string Name;

        public Frame()
        {
            Parts = EnumerableHelper.Array<Part>(16);
            Name = string.Empty;
        }

        public Part[] Parts { get; private set; }
    }
}