namespace MapEditor.Gui.Controls
{
    partial class FlipTextButton<T>
    {
        private class FlipTextOption
        {
            public readonly T Value;
            public readonly string Text;

            public FlipTextOption(T value, string text)
            {
                Value = value;
                Text = text;
            }
        }
    }
}