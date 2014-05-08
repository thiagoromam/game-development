using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses
{
    public class MapSegment
    {
        public Vector2 Location;
        public int Index { get; set; }

        public MapSegment() { }
        public MapSegment(int index, Vector2 location)
        {
            Index = index;
            Location = location;
        }
    }
}