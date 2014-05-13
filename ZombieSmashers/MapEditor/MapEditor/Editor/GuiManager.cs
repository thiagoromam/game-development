using GraphicalUserInterfaceLib;
using MapEditor.Editor.Controls.File;
using MapEditor.Editor.Controls.Map;
using MapEditor.Editor.Controls.Map.Ledge;

namespace MapEditor.Editor
{
    public class EditorGuiManager : GuiManager
    {
        public EditorGuiManager()
        {
            AddButtons();
            AddFlipButtons();
            AddTextEditors();
            AddLedgePallet();
        }

        private void AddLedgePallet()
        {
            var ledgePallet = new LedgePallete(520, 50);
            AddComponent(ledgePallet);
            AddTextControl(ledgePallet);
        }

        private void AddButtons()
        {
            AddButton(new SaveButton(5, 65));
            AddButton(new LoadButton(40, 65));
        }

        private void AddFlipButtons()
        {
            AddFlipButton(new MapLayerButton(5, 5));
            AddFlipButton(new DrawingModeButton(5, 25));
        }

        private void AddTextEditors()
        {
            AddTextEditor(new MapPathEditor(5, 45));
        }
    }
}