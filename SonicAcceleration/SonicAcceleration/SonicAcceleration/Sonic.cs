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
        private bool _invertingDirection;

        public Sonic()
        {
            Color = Color.White;
            _animations = new AnimationsManager { CurrentType = AnimationType.Idle };
            Right = true;
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
        public bool Right { get; private set; }

        public float Rotation
        {
            get
            {
                var rotation = _rotation + _animations.Current.FrameInformation.Rotation;
                return Right ? rotation : -rotation;
            }
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
            UpdateVelocity(gameTime);
            UpdateAnimation(gameTime);
        }

        private void UpdateVelocity(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            
            if (state.IsKeyDown(Keys.Right))
            {
                if (Right)
                    IncreaseVelocity(gameTime);
                else if (InvertDirection())
                    Right = true;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                if (!Right)
                    IncreaseVelocity(gameTime);
                else if (InvertDirection())
                    Right = false;
            }
            else
            {
                _invertingDirection = false;

                if (_velocity.X > 4)
                    _velocity.X *= 0.98f;
                else if (_velocity.X > 0.1f)
                    _velocity.X *= 0.97f;
                else
                {
                    _velocity.X *= 0.5f;
                    if (_velocity.X < 0.000001f)
                        _velocity.X = 0;
                }
            }
        }
        private void IncreaseVelocity(GameTime gameTime)
        {
            if (_velocity.X < 10)
                _velocity.X += 1.25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        private bool InvertDirection()
        {
            _velocity.X *= 0.6f;
            _invertingDirection = _velocity.X > 0.0001f;

            return !_invertingDirection;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (_invertingDirection)
            {
                _animations.CurrentType = AnimationType.Stopping;
            }
            else if (_velocity.X > 0)
            {
                _inativeTime = 0;

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