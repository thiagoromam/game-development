using System;
using Microsoft.Xna.Framework;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class Walking : IAnimation
    {
        private readonly IAnimation _animation;

        public Walking()
        {
            const int milliseconds = 125;

            IAnimationFrame[] frames =
            {
                new AnimationFrame(new FrameInformation(134, 152, 26, 40), milliseconds),
                new AnimationFrame(new FrameInformation(130, 607, 40, 39, MathHelper.PiOver2), milliseconds),
                new AnimationFrame(new FrameInformation(174, 152, 39, 40), milliseconds),
                new AnimationFrame(new FrameInformation(58, 675, 40, 26, MathHelper.PiOver2), milliseconds),
                new AnimationFrame(new FrameInformation(399, 474, 40, 28, MathHelper.PiOver2), milliseconds),
                new AnimationFrame(new FrameInformation(279, 152, 37, 40), milliseconds),
                new AnimationFrame(new FrameInformation(228, 152, 38, 40), milliseconds),
                new AnimationFrame(new FrameInformation(98, 152, 26, 40), milliseconds), 
            };
            _animation = new Animation(frames);
        }

        public FrameInformation FrameInformation
        {
            get { return _animation.FrameInformation; }
        }
        public int Iterations
        {
            get { return _animation.Iterations; }
        }

        public void Reset()
        {
            _animation.Reset();
        }

        public void Initialize()
        {
            _animation.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }
    }
}