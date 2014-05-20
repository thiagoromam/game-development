using Microsoft.Xna.Framework;

namespace GraphicalUserInterfaceLib.Controls
{
    partial class TextButtonList<T>
    {
        public class TextButtonOption
        {
            public T Value;
            public string Text;
            public readonly Vector2 Position;

            public TextButtonOption(T value, string text, Vector2 position)
            {
                Value = value;
                Text = text;
                Position = position;
            }
            public TextButtonOption(Vector2 position)
            {
                Position = position;
            }
        }
    }
}