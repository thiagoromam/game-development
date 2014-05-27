using GraphicalUserInterfaceLib.Api;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public class KeyFramePalette : IControlComponent, IControl
    {
        private readonly KeyFramesScroll _keyFramesScroll;
        private readonly KeyFrameSelector _keyFramesSelector;
        private const int YIncrement = 15;
        private const int X = 5;
        private const int Y = 250;

        public KeyFramePalette()
        {
            _keyFramesScroll = new KeyFramesScroll(13, X + 163, Y);
            _keyFramesSelector = new KeyFrameSelector(X, Y, YIncrement, _keyFramesScroll);
        }

        public void Update()
        {
            _keyFramesScroll.Update();
            _keyFramesSelector.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _keyFramesScroll.Draw(spriteBatch);
            _keyFramesSelector.Draw();
        }
    }
}