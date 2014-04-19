using Microsoft.Xna.Framework;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class Running : IAnimation
    {
        private readonly IAnimation _animation;

        public Running()
        {
            const int milliseconds = 85;
            IAnimationFrame[] frames =
            {
                new AnimationFrame(new FrameInformation(103, 206, 32, 36), milliseconds),
                new AnimationFrame(new FrameInformation(60, 206, 32, 36), milliseconds),
                new AnimationFrame(new FrameInformation(462, 765, 32, 36), milliseconds),
                new AnimationFrame(new FrameInformation(16, 206, 32, 36), milliseconds), 
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