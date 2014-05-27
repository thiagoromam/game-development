using System;
using Helpers;

namespace CharacterEditor.Character
{
    public class Frame
    {
        public const int PartsCount = 16;
        private string _name;

        public Frame()
        {
            Parts = EnumerableHelper.Array<Part>(PartsCount);
            _name = string.Empty;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnNameChanged();
            }
        }

        public Part[] Parts { get; private set; }

        public event Action NameChanged;
        public event Action PartsChanged;

        private void OnNameChanged()
        {
            var handler = NameChanged;
            if (handler != null) handler();
        }
        public void NotifyPartsChanged()
        {
            var handler = PartsChanged;
            if (handler != null) handler();
        }
    }
}