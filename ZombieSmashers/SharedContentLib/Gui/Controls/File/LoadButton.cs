namespace SharedLib.Gui.Controls.File
{
    public abstract class LoadButton : IconButton
    {
        protected LoadButton(int x, int y) : base(4, x, y)
        {
            Click = Load;
        }

        protected abstract void Load();
    }
}