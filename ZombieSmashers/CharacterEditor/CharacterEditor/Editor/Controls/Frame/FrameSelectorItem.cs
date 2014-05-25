namespace CharacterEditor.Editor.Controls.Frame
{
    public partial class FrameSelector
    {
        private class FrameSelectorItem
        {
            public readonly TextButtonOption Option;
            public int FrameIndex;

            public FrameSelectorItem(TextButtonOption option, int frameIndex)
            {
                FrameIndex = frameIndex;
                Option = option;
            }
        }
    }
}