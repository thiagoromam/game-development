using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Icons
{
    partial class IconsPalette
    {
        private struct IconPaletteItem
        {
            public readonly int Index;
            public readonly int TextureIndex;
            public readonly Rectangle Source;
            public readonly Rectangle Destination;

            public IconPaletteItem(int index, int textureIndex, Rectangle source, Rectangle destination)
            {
                Index = index;
                TextureIndex = textureIndex;
                Source = source;
                Destination = destination;
            }
        }
    }
}