using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Ioc.Api.Editor
{
    public interface IPartsPalleteComponent
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}