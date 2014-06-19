using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieSmashers.Input
{
    public class GamePadControlInput : IControlInput
    {
        private GamePadState _curState;
        private GamePadState _prevState;

        public bool KeyLeftPressing { get; private set; }
        public bool KeyRightPressing { get; private set; }
        public bool KeyUpPressing { get; private set; }
        public bool KeyUpPressed { get; private set; }
        public bool KeyDownPressing { get; private set; }
        public bool KeyDownPressed { get; private set; }
        public bool KeyJumpPressed { get; private set; }
        public bool KeyAttackPressed { get; private set; }
        public bool KeySecondaryPressed { get; private set; }
        public bool KeyStartPressed { get; private set; }
        
        public void Update()
        {
            _curState = GamePad.GetState(PlayerIndex.One);

            KeyLeftPressing = false;
            KeyRightPressing = false;
            KeyUpPressing = false;
            KeyDownPressing = false;

            if (_curState.ThumbSticks.Left.X < -0.1f)
                KeyLeftPressing = true;
            else if (_curState.ThumbSticks.Left.X > 0.1f)
                KeyRightPressing = true;

            if (_curState.ThumbSticks.Left.Y < -0.1f)
            {
                KeyDownPressing = true;
                KeyDownPressed = _prevState.ThumbSticks.Left.Y >= 0;
            }
            else if (_curState.ThumbSticks.Left.Y > 0.1f)
            {
                KeyUpPressing = true;
                KeyUpPressed = _prevState.ThumbSticks.Left.Y <= 0;
            }

            KeyJumpPressed = _curState.Buttons.A == ButtonState.Pressed && _prevState.Buttons.A == ButtonState.Released;
            KeyAttackPressed = _curState.Buttons.Y == ButtonState.Pressed && _prevState.Buttons.Y == ButtonState.Released;
            KeySecondaryPressed = _curState.Buttons.X == ButtonState.Pressed && _prevState.Buttons.X == ButtonState.Released;
            KeyStartPressed = _curState.Buttons.Start == ButtonState.Pressed && _prevState.Buttons.Start == ButtonState.Released;

            _prevState = _curState;
        }
    }
}