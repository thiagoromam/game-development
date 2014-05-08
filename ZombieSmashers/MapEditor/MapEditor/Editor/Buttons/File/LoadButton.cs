using MapEditor.Gui.Controls;

namespace MapEditor.Editor.Buttons.File
{
    public class LoadButton : Button
    {
        public LoadButton(int x, int y) : base(4, x, y) { }

        public override void Update()
        {
            base.Update();

            if (Clicked)
                Load();
        }

        private void Load()
        {
            
        }
    }
}