using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public interface IAnimation
    {
        FrameInformation FrameInformation { get; }
        int Iterations { get; }

        void Reset();
        void Initialize();
        void Update(GameTime gameTime);
    }
}