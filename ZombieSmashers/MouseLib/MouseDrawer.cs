using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;

namespace MouseLib
{
    internal class MouseDrawer : IMouseDrawer
    {
        private readonly IMouseInput _mouseInput;

        public MouseDrawer(IMouseInput mouseInput)
        {
            _mouseInput = mouseInput;
        }

        public Texture2D Texture { set; private get; }
        public Rectangle Source { set; private get; }
        public Vector2 Origin { set; private get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, _mouseInput.Position, Source, Color.White, 0, Origin, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}