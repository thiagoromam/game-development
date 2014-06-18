using System;

namespace ZombieSmashers.MapClasses
{
    public class MapFlags
    {
        private readonly String[] _flags;

        public MapFlags(int size)
        {
            _flags = new String[size];
            for (var i = 0; i < _flags.Length; i++) _flags[i] = "";
        }

        public bool GetFlag(String flag)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _flags.Length; i++)
            {
                if (_flags[i] == flag)
                    return true;
            }
            return false;
        }

        public void SetFlag(String flag)
        {
            if (GetFlag(flag)) return;
            for (var i = 0; i < _flags.Length; i++)
            {
                if (_flags[i] == "")
                {
                    _flags[i] = flag;
                    return;
                }
            }
        }
    }
}