using Microsoft.Xna.Framework;

namespace ZombieSmashers.MapClasses
{
    class Ledge
    {
        private readonly Vector2[] _nodes = new Vector2[16];

        public int TotalNodes { get; set; }
        public int Flags { get; set; }
        public Vector2[] Nodes
        {
            get { return _nodes; }
        }
    }
}
