using System.Globalization;
using CharacterEditor.Character;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;
using TextLib.Api;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public class DurationControl : IControlComponent, ITextControl
    {
        private readonly TextButton _subtractButton;
        private readonly TextButton _addButton;
        private readonly Vector2 _durationPosition;
        private readonly IText _text;

        public DurationControl(int x, int y)
        {
            _subtractButton = new TextButton("-", x, y) { Click = Subtract };
            _addButton = new TextButton("+", x + 30, y) { Click = Add };
            _durationPosition = new Vector2(x + 15, y);
            _text = DependencyInjection.Resolve<IText>();
        }

        public KeyFrame KeyFrame { get; set; }
        private bool HasFrame
        {
            get { return KeyFrame.FrameReference > -1; }
        }

        private void Subtract()
        {
            KeyFrame.Duration--;
        }
        private void Add()
        {
            KeyFrame.Duration++;
        }

        public void Update()
        {
            if (!HasFrame)
                return;

            _subtractButton.Update();
            _addButton.Update();
        }

        public void Draw()
        {
            if (!HasFrame)
                return;

            _subtractButton.Draw();
            _addButton.Draw();
            _text.Draw(KeyFrame.Duration.ToString(CultureInfo.InvariantCulture), _durationPosition);
        }
    }
}