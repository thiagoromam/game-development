using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public struct FrameInformation
    {
        public readonly Rectangle Source;
        public readonly Vector2 Origin;

        public FrameInformation(int x, int y, int width, int height)
        {
            Source = new Rectangle(x, y, width, height);
            Origin = new Vector2(width, height) / 2;
        }
    }
}