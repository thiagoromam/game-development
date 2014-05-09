using Microsoft.Xna.Framework;

namespace MapEditor.Gui.Controls
{
    partial class RadioButtonList<T>
    {
        private class RadioButtonOption
        {
            public readonly T Value;
            public readonly string Text;
            public readonly Vector2 Position;

            public RadioButtonOption(T value, string text, Vector2 position)
            {
                Value = value;
                Text = text;
                Position = position;
            }
        }
    }
}