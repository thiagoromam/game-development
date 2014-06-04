using Microsoft.Xna.Framework.Input;

namespace ZombieSmashers.Input
{
    public class KeyboardControlInput : IControlInput
    {
        private KeyboardState _curState;
        private KeyboardState _prevState;

        public bool KeyLeft { get; private set; }
        public bool KeyRight { get; private set; }
        public bool KeyUp { get; private set; }
        public bool KeyDown { get; private set; }
        public bool KeyJump { get; private set; }
        public bool KeyAttack { get; private set; }
        public bool KeySecondary { get; private set; }
        
        public void Update()
        {
            _curState = Keyboard.GetState();
            
            KeyLeft = false;
            KeyRight = false;
            KeyUp = false;
            KeyDown = false;

            if (_curState.IsKeyDown(Keys.A))
                KeyLeft = true;
            else if (_curState.IsKeyDown(Keys.D))
                KeyRight = true;

            if (_curState.IsKeyDown(Keys.W))
                KeyDown = true;
            else if (_curState.IsKeyDown(Keys.S))
                KeyUp = true;

            KeyJump = IsKeyClick(Keys.Space);
            KeyAttack = IsKeyClick(Keys.J);
            KeySecondary = IsKeyClick(Keys.K);

            _prevState = _curState;
        }

        private bool IsKeyClick(Keys key)
        {
            return _curState.IsKeyDown(key) && _prevState.IsKeyUp(key);
        }
    }
}