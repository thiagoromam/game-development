using GraphicalUserInterfaceLib.Api;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Editor.Controls.Animations
{
    public class AnimationsPalette : IControl, IControlComponent
    {
        private const int YIncrement = 15;
        private const int X = 5;
        private const int Y = 5;
        private readonly AnimationsScroll _animationsScroll;
        private readonly AnimationSelector _animationSelector;
        private readonly AnimationNameEditor _animationNameEditor;

        public AnimationsPalette()
        {
            _animationsScroll = new AnimationsScroll(15, X + 163, Y);
            _animationSelector = new AnimationSelector(X, Y, YIncrement, _animationsScroll);
            _animationNameEditor = new AnimationNameEditor(_animationSelector, _animationsScroll, X + 33, Y);
        }

        public void Update()
        {
            _animationsScroll.Update();
            _animationSelector.Update();
            _animationNameEditor.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animationSelector.Draw();
            _animationNameEditor.Draw();
            _animationsScroll.Draw(spriteBatch);
        }
    }
}