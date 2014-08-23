using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArrowShooting
{
    public class MainGame : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _arrowTexture;
        private Texture2D _pixel;
        private readonly List<Arrow> _arrows = new List<Arrow>();
        private MouseInput _mouse;
        private Vector2 _initialPosition;
        private ReferenceLine _referenceLine;
        private Texture2D _stubTexture;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _mouse = new MouseInput();
            _referenceLine = new ReferenceLine(_mouse, _pixel, _stubTexture);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _arrowTexture = Content.Load<Texture2D>("arrow");
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new [] { Color.Black });
            _stubTexture = Content.Load<Texture2D>("lineStub");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mouse.Update();
            _referenceLine.Update();

            if (_mouse.LeftDown)
            {
                _initialPosition = _mouse.Position;
            }
            else if (_mouse.LeftUp)
            {
                var direction = _mouse.Position - _initialPosition;
                var angle = (float)Math.Atan2(direction.Y, direction.X);

                var arrow = new Arrow(_initialPosition, angle, direction.Length() * 3, _arrowTexture);
                _arrows.Add(arrow);
            }

            var frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var arrow in _arrows)
            {
                if (arrow == null)
                    break;

                arrow.Update(frameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _referenceLine.Draw(_spriteBatch);

            foreach (var arrow in _arrows)
            {
                if (arrow == null)
                    break;

                arrow.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
