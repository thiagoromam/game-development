using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SonicAcceleration
{
    public class Sonic
    {
        private Dictionary<AnimationType, Animation> _animations;

        public Sonic()
        {
            Color = Color.White;
            Right = true;
        }

        public Texture2D Texture { get; private set; }
        public Rectangle? Source { get; private set; }
        public Vector2 Origin { get; private set; }
        public Color Color { get; private set; }
        public bool Right { get; private set; }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("sonic");
        }

        public void Initialize()
        {
            _animations = new Dictionary<AnimationType, Animation>();

            var idleSources = new Rectangle[] { };
            var runningSlowlySources = new Rectangle[] { };
            var runningSources = new Rectangle[] { };
            var runningFastSources = new Rectangle[] { };

            _animations[AnimationType.Idle] = new Animation(idleSources);
            _animations[AnimationType.RunningSlowly] = new Animation(runningSlowlySources);
            _animations[AnimationType.Running] = new Animation(runningSources);
            _animations[AnimationType.RunningFast] = new Animation(runningFastSources);
        }

        enum AnimationType
        {
            Idle,
            RunningSlowly,
            Running,
            RunningFast
        }
    }
}