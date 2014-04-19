using Microsoft.Xna.Framework;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class Idle : IAnimation
    {
        private readonly IAnimation _animation;

        public Idle()
        {
            _animation = new Animation(new FrameInformation(9, 6, 30, 39));
        }

        public FrameInformation FrameInformation
        {
            get { return _animation.FrameInformation; }
        }
        public int Iterations
        {
            get { return _animation.Iterations; }
        }
        public float VelocityFactor
        {
            set { _animation.VelocityFactor = value; }
        }

        public void Reset()
        {
            _animation.Reset();
        }

        public void Initialize()
        {
            _animation.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }
    }
}