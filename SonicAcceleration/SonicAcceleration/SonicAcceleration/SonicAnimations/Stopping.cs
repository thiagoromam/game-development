using Microsoft.Xna.Framework;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class Stopping : IAnimation
    {
        private readonly IAnimation _animation;

        public Stopping()
        {
            const int milliseconds = 30;
            IAnimationFrame[] frames =
            {
                new AnimationFrame(new FrameInformation(94, 106, 36, 37), milliseconds),
                new AnimationFrame(new FrameInformation(46, 106, 36, 37), milliseconds),
                new AnimationFrame(new FrameInformation(138, 106, 36, 37), 0) 
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