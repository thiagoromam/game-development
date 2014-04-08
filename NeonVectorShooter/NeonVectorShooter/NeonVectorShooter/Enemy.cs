using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
{
    public class Enemy : Entity
    {
        private static Random _random = new Random();

        private int _timeUntilStart;
        private readonly List<IEnumerator<int>> _behaviors;

        public int PointValue { get; private set; }

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
            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.IncreaseMultiplier();
        }

        public void AddBehavior(IEnumerable<int> behavior)
        {
            _behaviors.Add(behavior.GetEnumerator());
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

        public IEnumerable<int> MoveRandomly()
        {
            var direction = _random.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += _random.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (var i = 0; i < 6; ++i)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;

                    var bounds = GameRoot.Viewport.Bounds;
                    bounds.Inflate(-Image.Width, Image.Height);

                    if (bounds.Contains(Position.ToPoint()))
                    {
                        direction = (GameRoot.ScreenSize / 2 - Position).ToAngle();
                        direction += _random.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
                    }
                    
                    yield return 0;
                }
            }
        }

        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Seeker, position);
            enemy.AddBehavior(enemy.FollowPlayer());
            enemy.PointValue = 2;
            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehavior(enemy.MoveRandomly());
            enemy.PointValue = 1;
            return enemy;
        }

        public void HandleCollision(Entity other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }
    }
}