using Microsoft.Xna.Framework;

namespace MapEditor.Helpers
{
    public static class RectangleHelper
    {
        public static bool Contains(this Rectangle rectangle, Vector2 value)
        {
            return rectangle.Contains((int)value.X, (int)value.Y);
        }
    }
}