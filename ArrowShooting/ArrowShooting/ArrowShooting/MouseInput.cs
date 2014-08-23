using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArrowShooting
{
    public class MouseInput
    {
        private MouseState _lastState;
        private Vector2 _position;

        public bool LeftDown { get; private set; }
        public bool LeftPressed { get; set; }
        public bool LeftUp { get; private set; }
        public Vector2 Position
        {
            get { return _position; }
        }

        public void Update()
        {
            var state = Mouse.GetState();

            _position.X = state.X;
            _position.Y = state.Y;
            LeftPressed = state.LeftButton == ButtonState.Pressed;
            LeftDown = LeftPressed && _lastState.LeftButton == ButtonState.Released;
            LeftUp = state.LeftButton == ButtonState.Released && _lastState.LeftButton == ButtonState.Pressed;

            _lastState = state;
        } 
    }
}