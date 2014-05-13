using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Helpers
{
    public static class SpriteFontHelper
    {
        public static bool Intersects(this SpriteFont font, string text, Vector2 textPosition, float scale, Vector2 value)
        {
            if (value.X >= textPosition.X && value.Y >= textPosition.Y)
            {
                var end = textPosition + font.MeasureString(text) * scale;

                if (value.X <= end.X && value.Y <= end.Y)
                    return true;
            }

            return false;
        }
    }
}