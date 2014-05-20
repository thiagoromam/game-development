using System;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Character
{
    public class Part
    {
        public Vector2 Location;
        public float Rotation;
        public Vector2 Scaling;
        public int Flip;
        private int _index;
        public event Action IndexChanged;

        public Part()
        {
            Index = -1;
            Scaling = Vector2.One;
        }

        public int Index
        {
            get { return _index; }
            set
            {
                if (value == _index)
                    return;

                _index = value;
                OnIndexChanged();
            }
        }

        protected virtual void OnIndexChanged()
        {
            var handler = IndexChanged;
            if (handler != null) handler();
        }
    }
}