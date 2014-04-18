using Microsoft.Xna.Framework;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class Waiting : IAnimation
    {
        private readonly IAnimation _animation;

        public Waiting()
        {
            var waitingFrames = new IAnimationFrame[]
            {
                new AnimationFrame(new FrameInformation(118, 6, 32, 39), 500),
                new AnimationFrame(new FrameInformation(231, 6, 32, 39), 500)
            };

            var fallingAsleepFrames = new IAnimationFrame[]
            {
                new AnimationFrame(new FrameInformation(192, 6, 32, 39), 500),
                new AnimationFrame(new FrameInformation(271, 6, 32, 39), 500)
            };

            var sleepingFrame = new FrameInformation(81, 6, 31, 39);
            var wakeUpFrame = new FrameInformation(157, 6, 32, 39);

            _animation = new Animation(new IAnimationFrame[]
            {
                new LimitedAnimation(waitingFrames, 10),
                new LimitedAnimation(fallingAsleepFrames, 10),
                new AnimationFrame(sleepingFrame, 3000),
                new AnimationFrame(wakeUpFrame, 500)
            });
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