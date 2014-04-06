using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
{
    public class Enemy : Entity
    {
        private int _timeUntilStart;
        private readonly List<IEnumerator<int>> _behaviors;

        public bool IsActive
        {
            get { return _timeUntilStart <= 0; }
        }

        public Enemy(Texture2D image, Vector2 position)
        {
            _timeUntilStart = 60;
            Image = image;
            Position = position;
            Radius = image.Width / 2f;
            Color = Color.Transparent;
            _behaviors = new List<IEnumerator<int>>();
        }

        public override void Update()
        {
            if (IsActive)
            {
                ApplyBehaviors();
            }
            else
            {
                _timeUntilStart--;
                Color = Color.White * (1 - _timeUntilStart / 60);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            Velocity *= 0.98f;
        }

        public void WasShot()
        {
            IsExpired = true;
        }

        public void AddBehavior(IEnumerable<int> behavior)
        {
            _behaviors.Add(behavior);
        }

        private void ApplyBehaviors()
        {
            for (var i = 0; i < _behaviors.Count; i++)
            {
                if (!_behaviors[i].MoveNext())
                    _behaviors.RemoveAt(i--);
            }
        }

        public IEnumerable<int> FollowPlayer(float acceleration = 1)
        {
            while (true)
            {
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }
    }
}