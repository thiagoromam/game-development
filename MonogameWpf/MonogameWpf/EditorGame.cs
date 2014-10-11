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

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Vector2 _force;
        private Vector2 _position;

        public EditorGame(IntPtr handler)
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
            };

            var form = (Form)Control.FromHandle(Window.Handle);
            form.Enabled = false;
            form.Opacity = 0;

            _graphics.PreparingDeviceSettings += (s, e) => e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = handler;

            form.VisibleChanged += (s, e) => form.Visible = false;
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            base.Initialize();

            _position = new Vector2(50);
            _force = new Vector2(100);
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

            if (_position.X <= 0 || _position.X + 50 >= 800)
                _force.X = -_force.X;

            if (_position.Y <= 0 || _position.Y + 50 >= 600)
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