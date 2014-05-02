using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses
{
    public class SegmentDefinition
    {
        public readonly string Name;
        public readonly int Index;
        public readonly Rectangle Source;
        public readonly int Flags;
        public readonly Vector2 SourceLength;

        public SegmentDefinition(string name, int index, Rectangle source, int flags)
        {
            Name = name;
            Index = index;
            Source = source;
            Flags = flags;
            SourceLength = new Vector2(source.Width, source.Height);
        }
    }
}