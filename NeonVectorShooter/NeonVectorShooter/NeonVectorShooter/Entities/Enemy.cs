using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonVectorShooter.Contents;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.Managers;
using NeonVectorShooter.ParticleSystem;

namespace NeonVectorShooter.Entities
{
    public class Enemy : Entity
    {
        private static readonly Random Random = new Random();

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

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                ApplyBehaviors();
            }
            else
            {
                _timeUntilStart--;
                Color = Color.White * (1 - _timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            Velocity *= 0.8f;
        }

        public void WasShot()
        {
            IsExpired = true;

            var hue1 = Random.NextFloat(0, 6);
            var hue2 = (hue1 + Random.Next(0, 2)) %6f;
            var color1 = ColorUtil.HsvToColor(hue1, 0.5f, 1);
            var color2 = ColorUtil.HsvToColor(hue2, 0.5f, 1);

            for (var i = 0; i < 120; i++)
            {
                var speed = 18f * (1f - 1 / Random.NextFloat(1, 10));
                var state = new ParticleState
                {
                    Velocity = Random.Next(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                var color = Color.Lerp(color1, color2, Random.NextFloat(0, 1));
                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190, 1.5f, state);
            }

            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.IncreaseMultiplier();

            Sound.Explosion.Play(0.5f, Random.NextFloat(-0.2f, 0.2f), 0);
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
                if (!PlayerShip.Instance.IsDead)
                    Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public IEnumerable<int> MoveRandomly()
        {
            var direction = Random.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += Random.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (var i = 0; i < 6; ++i)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;

                    var bounds = GameRoot.Viewport.Bounds;
                    bounds.Inflate(-Image.Width / 2 - 1, Image.Height / 2 - 1);

                    if (bounds.Contains(Position.ToPoint()))
                    {
                        direction = (GameRoot.ScreenSize / 2 - Position).ToAngle();
                        direction += Random.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
                    }

                    yield return 0;
                }
            }
            // ReSharper disable once FunctionNeverReturns
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_timeUntilStart > 0)
            {
                var factor = _timeUntilStart / 60f;
                spriteBatch.Draw(Image, Position, null, Color.White * factor, Orientation, Origin, 2 - factor, SpriteEffects.None, 0);
            }

            base.Draw(spriteBatch);
        }
    }
}