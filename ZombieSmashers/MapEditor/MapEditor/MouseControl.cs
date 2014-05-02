using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    public class MouseControl
    {
        private Vector2 _position;
        private Vector2 _previousPosition;
        private MouseState _lastState;

        public bool RightButtonClick { get; private set; }
        public bool RightButtonPressed { get; private set; }
        public bool MiddleButtonPressed { get; private set; }
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

            RightButtonPressed = state.LeftButton == ButtonState.Pressed;
            MiddleButtonPressed = state.MiddleButton == ButtonState.Pressed;
            RightButtonClick = _lastState.LeftButton == ButtonState.Pressed && !RightButtonPressed;

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