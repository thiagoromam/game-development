using GraphicalUserInterfaceLib.Controls;

namespace CharacterEditor.Editor.Controls.Parts
{
    public class MirrorButton : FlipTextButton<int>
    {
        private Character.Part _part;

        public MirrorButton(int x, int y) : base(x, y)
        {
            AddOption(0, "(n)");
            AddOption(1, "(m)");

            Change = v => _part.Flip = v;
        }

        public void UpdateControl(Character.Part part)
        {
            _part = part;
            Value = part.Flip;
        }
    }
}