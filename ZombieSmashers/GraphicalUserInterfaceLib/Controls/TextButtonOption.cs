using Microsoft.Xna.Framework;

namespace GraphicalUserInterfaceLib.Controls
{
    partial class TextButtonList<T>
    {
        private class TextButtonOption
        {
            public readonly T Value;
            public readonly string Text;
            public readonly Vector2 Position;

            public TextButtonOption(T value, string text, Vector2 position)
            {
                Value = value;
                Text = text;
                Position = position;
            }
        }
    }
}