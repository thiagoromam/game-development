using System.Linq;
using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public class LimitedAnimation : IAnimation, IAnimationFrame
    {
        private readonly int _maxIterations;
        private readonly Animation _animation;
        private readonly IAnimationFrame _firstFrame;
        private readonly IAnimationFrame _lastFrame;
        private readonly int _totalMilliseconds;

        public LimitedAnimation(IAnimationFrame[] frames, int iterations)
        {
            _maxIterations = iterations;
            _animation = new Animation(frames);
            _totalMilliseconds = frames.Sum(f => f.TotalMilliseconds) * iterations;
            _firstFrame = frames.First();
            _lastFrame = frames.Last();
        }

        public FrameInformation FrameInformation
        {
            get { return _animation.FrameInformation; }
        }
        public int Iterations
        {
            get { return _animation.Iterations; }
        }
        public float VelocityFactor
        {
            set { _animation.VelocityFactor = value; }
        }
        int IAnimationFrame.TotalMilliseconds
        {
            get { return _totalMilliseconds; }
        }
        int IAnimationFrame.MillisecondsExceeded
        {
            get { return _lastFrame.MillisecondsExceeded; }
        }
        public bool Finished
        {
            get { return _animation.Iterations == _maxIterations; }
        }

        public void Reset()
        {
            _animation.Reset();
        }

        public void Initialize()
        {
            _animation.Initialize();
        }

        void IAnimationFrame.Reset(int? millisecondsExceededFromOtherFrame)
        {
            _animation.Reset();
            _firstFrame.Reset(millisecondsExceededFromOtherFrame);
        }

        public void Update(GameTime gameTime)
        {
            if (!Finished)
                _animation.Update(gameTime);
        }
    }
}
