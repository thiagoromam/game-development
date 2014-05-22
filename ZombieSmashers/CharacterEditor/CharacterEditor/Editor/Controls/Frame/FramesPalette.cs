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

        public FramesPalette()
        {
            _frameScroll = new FrameScroll(X + 170, Y);
            _frameSelector = new FrameSelector(X, Y, YIncrement, _frameScroll);
        }

        public void Update()
        {
            _frameSelector.Update();
            _frameScroll.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _frameSelector.Draw();
            _frameScroll.Draw(spriteBatch);
        }
    }
}