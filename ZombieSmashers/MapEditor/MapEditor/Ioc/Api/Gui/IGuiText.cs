using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Ioc.Api.Gui
{
    public interface IGuiText
    {
        bool MouseIntersects(string text, Vector2 position);
        void Draw(string text, Vector2 position);
        void Draw(string text, Vector2 position, Color color);
    }
}