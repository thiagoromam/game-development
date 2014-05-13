using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MouseLib.Api
{
    public interface IMouseDrawer
    {
        Texture2D Texture { set; }
        Rectangle Source { set; }
        Vector2 Origin { set; }

        void Draw(SpriteBatch spriteBatch);
    }
}