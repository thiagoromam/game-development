using System;
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
            var state = Keyboard.GetState();

            var keyPressed = false;

            if (state.IsKeyDown(Keys.Right))
            {
                Right = true;
                keyPressed = true;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                Right = false;
                keyPressed = true;
            }

            var velocityX = Math.Abs(_velocity.X);

            if (keyPressed)
            {
                if (velocityX < 10)
                {
                    var velocityToIncrease = 1.25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _velocity.X += Right ? velocityToIncrease : -velocityToIncrease;
                }

                velocityX = Math.Abs(_velocity.X);
            }
            else
            {
                if (velocityX > 4)
                    _velocity.X *= 0.98f;
                else if (velocityX > 0.1f)
                    _velocity.X *= 0.97f;
                else
                    _velocity.X *= 0.5f;
            }

            if (velocityX > 0.00001f)
            {
                _inativeTime = 0;

                if (velocityX < 4)
                {
                    _animations.CurrentType = AnimationType.Walking;
                    _animations.Current.VelocityFactor = 1 - (velocityX * 0.1625f);
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