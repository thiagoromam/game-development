using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombieSmashers.Shakes
{
    public class Rumble
    {
        private Vector2 _rumbleValue = Vector2.Zero;
        private readonly PlayerIndex _playerIndex;

        public Rumble(int idx)
        {
            _playerIndex = (PlayerIndex) idx;
        }

        public void Update()
        {
            if (_rumbleValue.X > 0f)
            {
                _rumbleValue.X -= Game1.FrameTime;
                if (_rumbleValue.X < 0f) _rumbleValue.X = 0f;
            }
            if (_rumbleValue.Y > 0f)
            {
                _rumbleValue.Y -= Game1.FrameTime;
                if (_rumbleValue.Y < 0f) _rumbleValue.Y = 0f;
            }
            GamePad.SetVibration(_playerIndex, _rumbleValue.X, _rumbleValue.Y);
        }

        public float Left
        {
            get { return _rumbleValue.X; }
            set { _rumbleValue.X = value; }
        }
        public float Right
        {
            get { return _rumbleValue.Y; }
            set { _rumbleValue.Y = value; }
        }
    }
}