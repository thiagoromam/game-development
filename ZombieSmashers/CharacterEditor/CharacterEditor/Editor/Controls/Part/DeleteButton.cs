using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.Part
{
    public class DeleteButton : TextButton
    {
        private Character.Part _part;

        public DeleteButton(int x, int y) : base("(x)", x, y)
        {
            Click = () => _part.Index = -1;
        }

        public void UpdateControl(Character.Part part)
        {
            _part = part;
        }
    }
}