using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SonicAcceleration
{
    public class SonicControlTracker
    {
        private Sonic _sonic;
        private KeyboardState _lastState;

        public SonicControlTracker(Sonic sonic)
        {
            _sonic = sonic;
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (_lastState.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.D))

            _lastState = state;
        }
    }
}
