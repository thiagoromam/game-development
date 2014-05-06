using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses
{
    public class Ledge
    {
        public int TotalNodes;
        public int Flags;

        public Ledge()
        {
            Nodes = new Vector2[16];
        }

        public Vector2[] Nodes { get; private set; }
    }
}