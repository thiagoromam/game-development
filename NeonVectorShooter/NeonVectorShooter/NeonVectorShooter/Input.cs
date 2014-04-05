using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NeonVectorShooter
{
    public static class Input
    {
        private static KeyboardState _keyboardState, _lastKeyboardState;
        private static MouseState _mouseState, _lastMouseState;
        private static GamePadState _gamePadState, _lastGamePadState;
        private static bool _isAimingWithMouse;

        public static Vector2 MousePosition
        {
            get { return new Vector2(_mouseState.X, _mouseState.Y); }
        }

        public static void Update()
        {
            _lastKeyboardState = _keyboardState;
            _lastMouseState = _mouseState;
            _lastGamePadState = _gamePadState;

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _gamePadState = GamePad.GetState(PlayerIndex.One);

            if (new [] { Keys.Left, Keys.Up, Keys.Right, Keys.Down, }.Any(k => _keyboardState.IsKeyDown(k) || _gamePadState.ThumbSticks.Right != Vector2.Zero))
                _isAimingWithMouse = false;
            else if (MousePosition != new Vector2(_lastMouseState.X, _lastMouseState.Y))
                _isAimingWithMouse = true;
        }

        public static bool WasKeyPressed(Keys key)
        {
            return _lastKeyboardState.IsKeyUp(key) && _keyboardState.IsKeyDown(key);
        }

        public static bool WasButtonPressed(Buttons button)
        {
            return _lastGamePadState.IsButtonUp(button) && _gamePadState.IsButtonDown(button);
        }

        public static Vector2 GetMovimentDirection()
        {
            var direction = _gamePadState.ThumbSticks.Left;
            direction.Y *= -1;

            if (_keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            else if (_keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;

            if (_keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            else if (_keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;

            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            if (_isAimingWithMouse)
                return GetMouseAimDirection();

            var direction = _gamePadState.ThumbSticks.Right;
            direction.Y *= -1;

            if (_keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            else if (_keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;

            if (_keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            else if (_keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;

            if (direction != Vector2.Zero)
                direction.Normalize();

            return direction;
        }

        private static Vector2 GetMouseAimDirection()
        {
            var direction = MousePosition - PlayerShip.Instance.Position;

            if (direction != Vector2.Zero)
                direction.Normalize();

            return direction;
        }

        public static bool WasBombButtonPressed()
        {
            return WasButtonPressed(Buttons.LeftTrigger) || WasButtonPressed(Buttons.RightTrigger) || WasKeyPressed(Keys.Space);
        }
    }
}