using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Ioc.Api.Map
{
    public interface IMapComponent : IMapData
    {
        void Draw(SpriteBatch spriteBatch, Vector2 scroll);
        int GetHoveredSegment(int layer, Vector2 scroll, Vector2 position);
    }
}