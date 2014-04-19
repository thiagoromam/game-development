using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SonicAcceleration.SonicAnimations;

namespace SonicAcceleration
{
    public class Sonic
    {
        private readonly AnimationsManager _animations;
        private int _inativeTime;
        private float _rotation;
        private Vector2 _velocity;

        public Sonic()
        {
            Color = Color.White;
            _animations = new AnimationsManager { CurrentType = AnimationType.Idle };
        }

        public Texture2D Texture { get; private set; }
        public Rectangle Source
        {
            get { return _animations.Current.FrameInformation.Source; }
        }
        public Vector2 Origin
        {
            get { return _animations.Current.FrameInformation.Origin; }
        }

        public float Rotation
        {
            get { return _rotation + _animations.Current.FrameInformation.Rotation; }
        }
        public Color Color { get; private set; }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("sonic");
        }

        public void Initialize()
        {
            _animations.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right))
            {
                if (_velocity.X < 6)
                    _velocity.X += 1.25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _velocity *= 0.98f;
            }

            if (_velocity.X < 0.0001f)
                _velocity.X = 0;
            else
                _inativeTime = 0;


            if (_velocity.X > 0)
            {
                if (_velocity.X < 4)
                {
                    _animations.CurrentType = AnimationType.Walking;
                    _animations.Current.VelocityFactor = 1 - (_velocity.X * 0.1625f);
                }
                else
                {
                    _animations.CurrentType = AnimationType.Running;
                }
            }
            else
            {
                _animations.CurrentType = _inativeTime <= 10000 ? AnimationType.Idle : AnimationType.Waiting;
                _inativeTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            _animations.Current.Update(gameTime);
        }
    }
}