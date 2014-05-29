namespace SharedLib.Gui.Controls.File
{
    public abstract class SaveButton : IconButton
    {
        protected SaveButton(int x, int y) : base(3, x, y)
        {
            Click = Save;
        }

        protected abstract void Save();
    }
}