using GraphicalUserInterfaceLib.Api;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Editor.Controls.Frames
{
    public class FramesPalette : IControlComponent, IControl
    {
        private const int YIncrement = 15;
        private const int X = 600;
        private const int Y = 280;
        private readonly FrameSelector _frameSelector;
        private readonly FramesesScroll _framesScroll;
        private readonly FrameNameEditor _frameNameEditor;
        private readonly AddFrameButton _addFrameButton;

        public FramesPalette()
        {
            _framesScroll = new FramesesScroll(20, X + 170, Y);
            _frameSelector = new FrameSelector(X, Y, YIncrement, _framesScroll);
            _frameNameEditor = new FrameNameEditor(_frameSelector, _framesScroll, X + 41, Y);
            _addFrameButton = new AddFrameButton(_frameSelector, _framesScroll, X + 120, Y);
        }

        public void Update()
        {
            _framesScroll.Update();
            _frameSelector.Update();
            _frameNameEditor.Update();
            _addFrameButton.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _frameSelector.Draw();
            _frameNameEditor.Draw();
            _addFrameButton.Draw();
            _framesScroll.Draw(spriteBatch);
        }
    }
}