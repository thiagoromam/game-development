using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public class Animation : IAnimation
    {
        private readonly IAnimationFrame[] _frames;
        private int _index;

        public Animation(IAnimationFrame[] frames)
        {
            _frames = frames;
        }
        public Animation(FrameInformation source)
        {
            _frames = new IAnimationFrame[] { new AnimationFrame(source, 0) };
        }

        private IAnimationFrame CurrentFrame
        {
            get { return _frames[_index]; }
        }
        public FrameInformation FrameInformation
        {
            get { return CurrentFrame.FrameInformation; }
        }
        public int Iterations { get; private set; }

        public void Reset()
        {
            _index = 0;
            Iterations = 0;
            CurrentFrame.Reset();
        }

        public void Initialize()
        {
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentFrame.TotalMilliseconds == 0)
                return;

            CurrentFrame.Update(gameTime);

            if (CurrentFrame.Finished)
                GoToNextFrame();
        }

        private void GoToNextFrame()
        {
            var millisecondsExceeded = CurrentFrame.MillisecondsExceeded;
            _index++;
            if (_index == _frames.Length)
            {
                _index = 0;
                Iterations++;
            }

            CurrentFrame.Reset(millisecondsExceeded);
        }
    }
}