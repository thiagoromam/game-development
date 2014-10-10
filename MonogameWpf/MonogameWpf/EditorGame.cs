using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameWpf
{
    public class EditorGame : Game
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly GraphicsDeviceManager _graphics;
        private readonly IntPtr _handler;
        
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Vector2 _position = new Vector2(50);
        private Vector2 _force;

        public EditorGame(IntPtr handler)
        {
            _handler = handler;
            
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
            };

            _graphics.PreparingDeviceSettings += (s, e) =>
            {
                var form = (Form)Control.FromHandle(Window.Handle);
                form.Enabled = false;
                form.Opacity = 0;

                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _handler;
            };

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _force = new Vector2(50);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            _position += _force * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_position.X <= 0 || _position.X >= 800)
                _force.X = -_force.X;

            if (_position.Y <= 0 || _position.Y >= 600)
                _force.Y = -_force.Y;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, 50, 50), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}