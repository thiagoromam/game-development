using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RotationPoint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _rectangleTexture;
        private Vector2 _rectangleOffset;
        private Vector2 _rectanglePosition;
        private float _rotation;
        private Vector2 _rectangleOrigin;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _rectangleTexture = Content.Load<Texture2D>("rect");

            _rectanglePosition = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2;
            _rectangleOffset = new Vector2(_rectangleTexture.Width, _rectangleTexture.Height) / 2;
            _rectangleOrigin = Vector2.Zero;
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

            var totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.A))
                _rotation -= MathHelper.Pi * totalSeconds;
            else if (keyboard.IsKeyDown(Keys.D))
                _rotation += MathHelper.Pi * totalSeconds;

            var direction = Vector2.Zero;

            if (keyboard.IsKeyDown(Keys.Left))
                direction.X = -1;
            else if (keyboard.IsKeyDown(Keys.Right))
                direction.X = 1;

            if (keyboard.IsKeyDown(Keys.Up))
                direction.Y = -1;
            else if (keyboard.IsKeyDown(Keys.Down))
                direction.Y = 1;

            if (direction != Vector2.Zero)
            {
                var d = Math.Atan2(direction.Y, direction.X) - _rotation;
                var directionVector = new Vector2((float)Math.Cos(d), (float)Math.Sin(d));
                _rectangleOrigin = Vector2.Clamp(
                    _rectangleOrigin + 100 * totalSeconds * directionVector,
                    Vector2.Zero,
                    _rectangleOffset * 2
                );
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_rectangleTexture, _rectanglePosition, null, Color.White, _rotation, _rectangleOrigin, 1, SpriteEffects.None, 1);
            _spriteBatch.Draw(_rectangleTexture, _rectanglePosition, null, Color.Red, 0, _rectangleOffset, 0.1f, SpriteEffects.None, 1);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
