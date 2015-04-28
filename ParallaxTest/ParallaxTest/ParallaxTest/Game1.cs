using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParallaxTest
{
    // http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardInput _keyboard;
        private List<Layer> _layers;
        private Camera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _keyboard = new KeyboardInput();
            _camera = new Camera(GraphicsDevice.Viewport)/* { Limits = new Rectangle(0, 0, 3200, 600) }*/;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _layers = new List<Layer>
            {
                new Layer(_camera) { Parallax = new Vector2(0.0f, 1.0f) },
                new Layer(_camera) { Parallax = new Vector2(0.1f, 0.1f) },
                new Layer(_camera) { Parallax = new Vector2(0.2f, 0.2f) },
                new Layer(_camera) { Parallax = new Vector2(0.3f, 0.3f) },
                new Layer(_camera) { Parallax = new Vector2(0.4f, 0.4f) },
                new Layer(_camera) { Parallax = new Vector2(0.5f, 0.5f) },
                new Layer(_camera) { Parallax = new Vector2(0.6f, 0.6f) },
                new Layer(_camera) { Parallax = new Vector2(0.8f, 0.8f) },
                new Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) }
            };

            _layers[0].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer1") });
            _layers[1].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer2") });
            _layers[2].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer3") });
            _layers[3].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer4") });
            _layers[4].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer5") });
            _layers[5].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer6") });
            _layers[6].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer7") });
            _layers[7].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer8") });
            _layers[7].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer8"), Position = new Vector2(900, 0) });
            _layers[8].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer9") });
            _layers[8].Sprites.Add(new Sprite { Texture = Content.Load<Texture2D>("Layer9"), Position = new Vector2(1600, 0) });
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboard.Update();
            if (_keyboard.WasKeysPressed(Keys.Escape))
                Exit();

            var movement = 200f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_keyboard.IsKeyDown(Keys.Left))
                _camera.Position.X -= movement;
            else if (_keyboard.IsKeyDown(Keys.Right))
                _camera.Position.X += movement;

            if (_keyboard.IsKeyDown(Keys.Up))
                _camera.Position.Y -= movement;
            else if (_keyboard.IsKeyDown(Keys.Down))
                _camera.Position.Y += movement;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Layer layer in _layers)
                layer.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
