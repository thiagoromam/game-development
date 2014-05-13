namespace GraphicalUserInterfaceLib.Controls
{
    partial class FlipTextButton<T>
    {
        private class FlipTextOption
        {
            public readonly string Text;
            public readonly T Value;

            public FlipTextOption(T value, string text)
            {
                Value = value;
                Text = text;
            }
        }
    }
}