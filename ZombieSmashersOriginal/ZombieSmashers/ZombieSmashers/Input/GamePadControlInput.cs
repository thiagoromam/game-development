using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieSmashers.Input
{
    public class GamePadControlInput : IControlInput
    {
        private GamePadState _curState;
        private GamePadState _prevState;

        public bool KeyLeft { get; private set; }
        public bool KeyRight { get; private set; }
        public bool KeyUp { get; private set; }
        public bool KeyDown { get; private set; }
        public bool KeyJump { get; private set; }
        public bool KeyAttack { get; private set; }
        public bool KeySecondary { get; private set; }
        
        public void Update()
        {
            _curState = GamePad.GetState(PlayerIndex.One);

            KeyLeft = false;
            KeyRight = false;
            KeyUp = false;
            KeyDown = false;

            if (_curState.ThumbSticks.Left.X < -0.1f)
                KeyLeft = true;
            else if (_curState.ThumbSticks.Left.X > 0.1f)
                KeyRight = true;

            if (_curState.ThumbSticks.Left.Y < -0.1f)
                KeyDown = true;
            else if (_curState.ThumbSticks.Left.Y > 0.1f)
                KeyUp = true;

            KeyJump = _curState.Buttons.A == ButtonState.Pressed && _prevState.Buttons.A == ButtonState.Released;
            KeyAttack = _curState.Buttons.Y == ButtonState.Pressed && _prevState.Buttons.Y == ButtonState.Released;
            KeySecondary = _curState.Buttons.X == ButtonState.Pressed && _prevState.Buttons.X == ButtonState.Released;
            
            _prevState = _curState;
        }
    }
}