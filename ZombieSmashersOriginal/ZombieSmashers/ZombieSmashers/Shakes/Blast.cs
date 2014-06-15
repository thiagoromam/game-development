using Microsoft.Xna.Framework;

namespace ZombieSmashers.Shakes
{
    public class Blast
    {
        private float _val;
        public Vector2 Center;

        public void Update()
        {
            if (_val >= 0f)
                _val -= Game1.FrameTime * 5f;
            else if (_val < 0f)
                _val = 0f;
        }

        public float Value
        {
            get { return _val; }
            set { _val = value; }
        }
        public float Magnitude { get; set; }
    }
}