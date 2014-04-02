using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameWindows8Test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _surge;
        private Effect _effect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _surge = Content.Load<Texture2D>("surge");
            _effect = Content.Load<Effect>("RedChannelEffect");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _effect.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(_surge, Vector2.Zero, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
