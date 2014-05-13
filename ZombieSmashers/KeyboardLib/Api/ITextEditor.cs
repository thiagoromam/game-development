namespace KeyboardLib.Api
{
    public interface ITextEditor
    {
        string Text { get; set; }
        void RemoveFocus();
    }
}