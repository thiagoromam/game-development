using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Parts
{
    public class ResetButton : TextButton
    {
        private Character.Part _part;

        public ResetButton(int x, int y) : base("(r)", x, y)
        {
            Click = () => _part.Scaling = new Vector2(1.0f, 1.0f);
        }

        public void UpdateControl(Character.Part part)
        {
            _part = part;
        }
    }
}