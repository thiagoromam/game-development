using Microsoft.Xna.Framework;

namespace TextLib.Api
{
    public interface IText
    {
        bool MouseIntersects(string text, Vector2 position);
        void Draw(string text, Vector2 position);
        void Draw(string text, Vector2 position, Color color);
    }
}