using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public class AnimationFrame : IAnimationFrame
    {
        private int _millisecondsUntilNextFrame;

        public AnimationFrame(FrameInformation source, int totalMilliseconds)
        {
            FrameInformation = source;
            TotalMilliseconds = totalMilliseconds;
            _millisecondsUntilNextFrame = totalMilliseconds;
        }

        public FrameInformation FrameInformation { get; private set; }
        public int TotalMilliseconds { get; private set; }
        public int MillisecondsExceeded { get; private set; }
        public bool Finished
        {
            get { return _millisecondsUntilNextFrame <= 0; }
        }

        public void Reset(int? millisecondsExcededFromOtherFrame)
        {
            _millisecondsUntilNextFrame = TotalMilliseconds;
            _millisecondsUntilNextFrame -= millisecondsExcededFromOtherFrame ?? 0;

            MillisecondsExceeded = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (TotalMilliseconds == 0)
                return;

            _millisecondsUntilNextFrame -= gameTime.ElapsedGameTime.Milliseconds;
            if (_millisecondsUntilNextFrame > 0) return;

            MillisecondsExceeded = -_millisecondsUntilNextFrame;
        }
    }
}