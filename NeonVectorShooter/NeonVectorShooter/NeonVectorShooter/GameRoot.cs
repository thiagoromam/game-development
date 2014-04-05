/* http://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NeonVectorShooter
{
    public class GameRoot : Game
    {
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport
        {
            get { return Instance.GraphicsDevice.Viewport; }
        }
        public static Vector2 ScreenSize
        {
            get { return new Vector2(Viewport.Width, Viewport.Height); }
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Instance = this;
        }

        protected override void Initialize()
        {
            base.Initialize();

            EntityManager.Add(PlayerShip.Instance);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update();
            EntityManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(_spriteBatch);
            _spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
