using Microsoft.Xna.Framework;

namespace ZombieSmashers.Shakes
{
    public class Quake
    {
        private float _val;

        public void Update()
        {
            if (_val > 0f) _val -= Game1.FrameTime;
            else if (_val < 0f) _val = 0f;
        }

        public float Value
        {
            get { return _val; }
            set { _val = value; }
        }
        public Vector2 Vector
        {
            get
            {
                if (_val <= 0f) return Vector2.Zero;
                return Rand.GetRandomVector2(-_val, _val, -_val, _val)*10f;
            }
        }
    }
}