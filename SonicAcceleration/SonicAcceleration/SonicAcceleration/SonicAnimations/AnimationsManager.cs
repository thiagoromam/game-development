using System.Collections.Generic;
using SonicAcceleration.Animations;

namespace SonicAcceleration.SonicAnimations
{
    public class AnimationsManager
    {
        private readonly Dictionary<AnimationType, IAnimation> _animations;
        private AnimationType _currentType;

        public AnimationsManager()
        {
            _animations = new Dictionary<AnimationType, IAnimation>();
            _animations[AnimationType.Idle] = new Idle();
            _animations[AnimationType.Waiting] = new Waiting();
            _animations[AnimationType.Walking] = new Walking();
            _animations[AnimationType.Running] = new Running();
        }

        public AnimationType CurrentType
        {
            get { return _currentType; }
            set
            {
                if (_currentType == value)
                    return;

                _currentType = value;
                if (_animations.ContainsKey(value))
                    Current.Reset();
            }
        }
        public IAnimation Current
        {
            get { return _animations[CurrentType]; }
        }

        public void Initialize()
        {
            foreach (var animation in _animations.Values)
                animation.Initialize();
        }
    }
}
