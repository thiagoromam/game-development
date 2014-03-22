using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirst2DPixelShader
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Effect[] _effects;
        private KeyboardState _lastState;
        private int _currentEffect;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Content.Load<Texture2D>("surge");
            _effects = new[]
            {
                Content.Load<Effect>("GrayScaleEffect"),
                Content.Load<Effect>("HighContrastEffect"),
                Content.Load<Effect>("NegativeEffect")
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyUp(Keys.A) && _lastState.IsKeyDown(Keys.A))
            {
                _currentEffect--;
                if (_currentEffect < 0)
                    _currentEffect = _effects.Length - 1;
            }
            else if (state.IsKeyUp(Keys.D) && _lastState.IsKeyDown(Keys.D))
            {
                _currentEffect++;
                if (_currentEffect == _effects.Length)
                    _currentEffect = 0;
            }

            _lastState = state;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);

            _effects[_currentEffect].CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Draw(_texture, new Vector2(50, 0), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
