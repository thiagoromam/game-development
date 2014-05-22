using CharacterEditor.Editor.Controls.Frame;
using CharacterEditor.Editor.Controls.Icons;
using CharacterEditor.Editor.Controls.Part;

namespace CharacterEditor.Editor
{
    public class GuiManager : GraphicalUserInterfaceLib.GuiManager
    {
        public GuiManager()
        {
            AddIconsPalette();
            AddPartsPalette();
            AddFramesPalette();
        }

        private void AddIconsPalette()
        {
            var iconsPalette = new IconsPalette();
            AddComponent(iconsPalette);
            AddControl(iconsPalette);
        }

        private void AddPartsPalette()
        {
            var partsPalette = new PartsPalette();
            AddComponent(partsPalette);
            AddControl(partsPalette);
        }

        private void AddFramesPalette()
        {
            var framesPalette = new FramesPalette();
            AddComponent(framesPalette);
            AddControl(framesPalette);
        }
    }
}