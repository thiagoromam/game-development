using Microsoft.Xna.Framework;

namespace SonicAcceleration.Animations
{
    public struct FrameInformation
    {
        public readonly Rectangle Source;
        public readonly float Rotation;
        public readonly Vector2 Origin;

        public FrameInformation(int x, int y, int width, int height, float rotation = 0)
        {
            Source = new Rectangle(x, y, width, height);
            Rotation = rotation;
            Origin = new Vector2(width, height) / 2;
        }
    }
}