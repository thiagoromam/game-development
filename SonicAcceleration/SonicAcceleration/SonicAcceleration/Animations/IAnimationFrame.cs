using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public interface IAnimationFrame
    {
        FrameInformation FrameInformation { get; }
        int TotalMilliseconds { get; }
        int MillisecondsExceeded { get; }
        bool Finished { get; }

        void Reset(int? millisecondsExceededFromOtherFrame = null);
        void Update(GameTime gameTime);
    }
}