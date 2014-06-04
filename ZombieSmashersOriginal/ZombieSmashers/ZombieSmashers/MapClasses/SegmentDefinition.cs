using Microsoft.Xna.Framework;

namespace ZombieSmashers.MapClasses
{
    public class SegmentDefinition
    {
        private Rectangle _srcRect;

        public SegmentDefinition(string name, int sourceIndex, Rectangle srcRect, int flags)
        {
            Name = name;
            SourceIndex = sourceIndex;
            SourceRect = srcRect;
            Flags = flags;
        }

        public string Name { get; set; }
        public int SourceIndex { get; set; }
        public Rectangle SourceRect
        {
            get { return _srcRect; }
            set { _srcRect = value; }
        }
        public int Flags { get; set; }
    }
}
