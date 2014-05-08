using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Ioc.Api.Input
{
    public interface IMouseComponent
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}