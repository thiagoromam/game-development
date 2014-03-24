/* http://blog.josack.com/2011/08/my-first-2d-pixel-shaders-part-3.html */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TextureReplaceEffect
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _surgeTexture;
        private Effect _effect;
        private Texture2D _rainbowTexture;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _surgeTexture = Content.Load<Texture2D>("surge");
            _rainbowTexture = Content.Load<Texture2D>("rainbow");
            _effect = Content.Load<Effect>("TextureReplaceEffect");
            _effect.Parameters["rainbow"].SetValue(_rainbowTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            _spriteBatch.Draw(_surgeTexture, Vector2.Zero, Color.White);

            _effect.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(_surgeTexture, new Vector2(50, 0), Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
