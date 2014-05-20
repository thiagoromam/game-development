using System;
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

        public event Action PartsChanged;

        public void NotifyPartsChanged()
        {
            var handler = PartsChanged;
            if (handler != null) handler();
        }
    }
}