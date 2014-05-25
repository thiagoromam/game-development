using GraphicalUserInterfaceLib.Api;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Editor.Controls.Frame
{
    public class FramesPalette : IControlComponent, IControl
    {
        private const int YIncrement = 15;
        private const int X = 600;
        private const int Y = 280;
        private readonly FrameSelector _frameSelector;
        private readonly FrameScroll _frameScroll;
        private readonly FrameNameEditor _frameNameEditor;
        private readonly AddFrameButton _addFrameButton;

        public FramesPalette()
        {
            _frameScroll = new FrameScroll(20, X + 170, Y);
            _frameSelector = new FrameSelector(X, Y, YIncrement, _frameScroll);
            _frameNameEditor = new FrameNameEditor(_frameSelector, _frameScroll, X + 41, Y);
            _addFrameButton = new AddFrameButton(_frameSelector, _frameScroll, X + 120, Y);
        }

        public void Update()
        {
            _frameSelector.Update();
            _frameScroll.Update();
            _frameNameEditor.Update();
            _addFrameButton.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _frameSelector.Draw();
            _frameScroll.Draw(spriteBatch);
            _frameNameEditor.Draw();
            _addFrameButton.Draw();
        }
    }
}