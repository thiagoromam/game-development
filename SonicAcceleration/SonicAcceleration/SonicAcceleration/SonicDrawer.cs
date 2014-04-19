using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicAcceleration
{
    public class SonicDrawer
    {
        private readonly Sonic _sonic;
        private Vector2 _position;

        public SonicDrawer(Sonic sonic)
        {
            _sonic = sonic;
        }

        public void Initialize()
        {
            var viewport = GameRoot.Instance.GraphicsDevice.Viewport;
            _position = new Vector2(viewport.Width, viewport.Height) / 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _sonic.Texture,
                _position,
                _sonic.Source,
                _sonic.Color,
                _sonic.Rotation,
                _sonic.Origin,
                2,
                _sonic.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
            );
        }
    }
}
