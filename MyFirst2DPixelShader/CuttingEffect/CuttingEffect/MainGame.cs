/* http://blog.josack.com/2011/08/my-first-2d-pixel-shaders-part-3.html */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CuttingEffect
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Effect _effect;
        private float _visiblePercent = .5f;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Content.Load<Texture2D>("surge");
            _effect = Content.Load<Effect>("CuttingEffect");
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.A))
                _visiblePercent -= (float) gameTime.ElapsedGameTime.TotalSeconds*0.5f;
            else if (state.IsKeyDown(Keys.D))
                _visiblePercent += (float) gameTime.ElapsedGameTime.TotalSeconds*0.5f;

            _visiblePercent = MathHelper.Clamp(_visiblePercent, 0, 100);
            _effect.Parameters["visiblePercent"].SetValue(_visiblePercent);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);

            _effect.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(_texture, new Vector2(50, 0), Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
