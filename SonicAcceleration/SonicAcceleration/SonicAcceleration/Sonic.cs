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
                _animations.CurrentType = AnimationType.Running;
                _inativeTime = 0;
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