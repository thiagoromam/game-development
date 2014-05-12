using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor.Input
{
    public class MouseControl : IMouseInput, IMouseComponent
    {
        private Vector2 _position;
        private Vector2 _previousPosition;
        private MouseState _lastState;

        public bool LeftButtonClick { get; private set; }
        public bool LeftButtonDown { get; private set; }
        public bool LeftButtonPressed { get; private set; }
        public bool MiddleButtonPressed { get; private set; }
        public bool RightButtonClick { get; set; }
        public Vector2 Position
        {
            get { return _position; }
        }
        public Vector2 PreviousPosition
        {
            get { return _previousPosition; }
        }

        public void Update()
        {
            var state = Mouse.GetState();

            _previousPosition.X = _lastState.X;
            _previousPosition.Y = _lastState.Y;
            _position.X = state.X;
            _position.Y = state.Y;

            LeftButtonPressed = state.LeftButton == ButtonState.Pressed;
            MiddleButtonPressed = state.MiddleButton == ButtonState.Pressed;
            LeftButtonDown = _lastState.LeftButton == ButtonState.Released && LeftButtonPressed;
            LeftButtonClick = _lastState.LeftButton == ButtonState.Pressed && !LeftButtonPressed;
            RightButtonClick = _lastState.RightButton == ButtonState.Pressed && state.RightButton == ButtonState.Released;

            _lastState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Art.Icons, Position, new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}