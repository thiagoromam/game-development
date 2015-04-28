using Microsoft.Xna.Framework.Input;

namespace ParallaxTest
{
    public class KeyboardInput
    {
        private KeyboardState _lastState;
        private KeyboardState _currentState;

        public void Update()
        {
            _lastState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public bool WasKeysPressed(Keys key)
        {
            return _lastState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }
    }
}