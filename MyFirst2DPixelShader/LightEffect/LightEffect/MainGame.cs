/* http://blog.josack.com/2011/08/my-first-2d-pixel-shaders-part-3.html */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LightEffect
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _surgeTexture;
        private Effect _effect;
        private RenderTarget2D _lightsTarget;
        private RenderTarget2D _mainTarget;
        private Texture2D _lighmask;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _surgeTexture = Content.Load<Texture2D>("surge");
            _lighmask = Content.Load<Texture2D>("lightmask");
            _effect = Content.Load<Effect>("LightEffect");

            var backBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var backBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            _lightsTarget = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight);
            _mainTarget = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight);
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
            GraphicsDevice.SetRenderTarget(_lightsTarget);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            _spriteBatch.Draw(_lighmask, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_lighmask, new Vector2(100, 0), Color.White);
            _spriteBatch.Draw(_lighmask, new Vector2(200, 200), Color.White);
            _spriteBatch.Draw(_lighmask, new Vector2(300, 300), Color.White);
            _spriteBatch.Draw(_lighmask, new Vector2(500, 200), Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(_mainTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_surgeTexture, new Vector2(100, 0), Color.White);
            _spriteBatch.Draw(_surgeTexture, new Vector2(250, 250), Color.White);
            _spriteBatch.Draw(_surgeTexture, new Vector2(550, 225), Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _effect.Parameters["lightMask"].SetValue(_lightsTarget);
            _effect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
