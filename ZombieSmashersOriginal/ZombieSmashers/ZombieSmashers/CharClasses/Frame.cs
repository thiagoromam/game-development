using System;

namespace ZombieSmashers.CharClasses
{
    public class Frame
    {
        private readonly Part[] _parts;
        public string Name;

        public Frame()
        {
            _parts = new Part[16];
            for (var i = 0; i < _parts.Length; i++)
                _parts[i] = new Part();

            Name = String.Empty;
        }

        public Part[] Parts
        {
            get { return _parts; }
        }
    }
}