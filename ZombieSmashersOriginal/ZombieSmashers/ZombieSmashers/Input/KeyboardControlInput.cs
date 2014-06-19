using Microsoft.Xna.Framework.Input;

namespace ZombieSmashers.Input
{
    public class KeyboardControlInput : IControlInput
    {
        private KeyboardState _curState;
        private KeyboardState _prevState;

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
            _curState = Keyboard.GetState();
            
            KeyLeftPressing = false;
            KeyRightPressing = false;
            KeyUpPressing = false;
            KeyDownPressing = false;

            if (_curState.IsKeyDown(Keys.A))
                KeyLeftPressing = true;
            else if (_curState.IsKeyDown(Keys.D))
                KeyRightPressing = true;

            if (_curState.IsKeyDown(Keys.W))
            {
                KeyUpPressing = true;
                KeyUpPressed = _prevState.IsKeyUp(Keys.W);
            }
            else if (_curState.IsKeyDown(Keys.S))
            {
                KeyDownPressing = true;
                KeyDownPressed = _prevState.IsKeyUp(Keys.S);
            }

            KeyJumpPressed = IsKeyClick(Keys.Space);
            KeyAttackPressed = IsKeyClick(Keys.J);
            KeySecondaryPressed = IsKeyClick(Keys.K);
            KeyStartPressed = IsKeyClick(Keys.Enter);

            _prevState = _curState;
        }

        private bool IsKeyClick(Keys key)
        {
            return _curState.IsKeyDown(key) && _prevState.IsKeyUp(key);
        }
    }
}