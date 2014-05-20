using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Icons
{
    partial class IconsPallete
    {
        private struct PalleteItem
        {
            public readonly int Index;
            public readonly int TextureIndex;
            public readonly Rectangle Source;
            public readonly Rectangle Destination;

            public PalleteItem(int index, int textureIndex, Rectangle source, Rectangle destination)
            {
                Index = index;
                TextureIndex = textureIndex;
                Source = source;
                Destination = destination;
            }
        }
    }
}