using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonVectorShooter.Contents;
using NeonVectorShooter.Helpers;
using NeonVectorShooter.Managers;
using NeonVectorShooter.ParticleSystem;

namespace NeonVectorShooter.Entities
{
    public class PlayerShip : Entity
    {
        private const int CooldownFrames = 6;
        private int _cooldownRemaining;
        private readonly Random _random;
        private int _framesUntilRespaw;

        private static PlayerShip _instance;
        public static PlayerShip Instance
        {
            get { return _instance ?? (_instance = new PlayerShip()); }
        }
        public bool IsDead
        {
            get { return _framesUntilRespaw > 0; }
        }

        private PlayerShip()
        {
            Image = Art.Player;
            Position = GameRoot.ScreenSize / 2f;
            Radius = 10;
            _random = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                if (--_framesUntilRespaw == 0)
                {
                    if (PlayerStatus.IsGameOver)
                    {
                        PlayerStatus.Reset();
                        Position = GameRoot.ScreenSize / 2;
                    }
                }

                return;
            }

            UpdateFire();
            UpdatePosition(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            const float speed = 8;
            Velocity += speed * Input.GetMovimentDirection();
            Position += Velocity;
            var halfSize = Size / 2;
            Position = Vector2.Clamp(Position, halfSize, GameRoot.ScreenSize - halfSize);

            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
                MakeExhaustFire(gameTime);
            }

            Velocity = Vector2.Zero;
        }

        private void UpdateFire()
        {
            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && _cooldownRemaining <= 0)
            {
                _cooldownRemaining = CooldownFrames;
                var aimAngle = aim.ToAngle();
                var aimQuaternion = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                var randomSpreed = _random.NextFloat(-0.04f, 0.04f) + _random.NextFloat(-0.04f, 0.04f);
                var velocity = MathUtil.FromPolar(aimAngle + randomSpreed, 11);

                var offset = Vector2.Transform(new Vector2(35, -8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, velocity));

                offset = Vector2.Transform(new Vector2(35, 8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, velocity));

                Sound.Shoot.Play(0.2f, _random.NextFloat(-0.2f, 0.2f), 0);
            }

            if (_cooldownRemaining > 0)
                _cooldownRemaining--;
        }

        public void Kill()
        {
            _framesUntilRespaw = PlayerStatus.IsGameOver ? 500 : 120;
            PlayerStatus.RemoveLife();

            CreateExplosion();
        }

        private void CreateExplosion()
        {
            var yellow = new Color(0.8f, 0.8f, 0.4f);

            for (var i = 0; i < 1200; i++)
            {
                var speed = 18f * (1f - 1f / _random.NextFloat(1f, 10f));
                var color = Color.Lerp(Color.White, yellow, _random.NextFloat(0, 1));

                var state = new ParticleState
                {
                    Velocity = _random.NextVector(speed, speed),
                    Type = ParticleType.None,
                    LengthMultiplier = 1
                };

                GameRoot.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190, 1.5f, state);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }

        private void MakeExhaustFire(GameTime gameTime)
        {
            var rotation = Quaternion.CreateFromYawPitchRoll(0, 0, Orientation);

            var t = gameTime.TotalGameTime.TotalSeconds;

            var baseVelocity = Velocity.ScaleTo(-3);
            var perpendicularVelocity = new Vector2(baseVelocity.Y, -baseVelocity.X) * (0.6f * (float)Math.Sin(t * 10));
            var sideColor = new Color(200, 38, 9);
            var midColor = new Color(255, 187, 30);
            var position = Position + Vector2.Transform(new Vector2(-25, 0), rotation);

            const float alpha = 0.7f;
            const int duration = 60;
            var scale = new Vector2(0.5f, 1);
            var whiteColor = Color.White * alpha;
            sideColor *= alpha;

            // middle particle stream
            var midVelocity = baseVelocity + _random.NextVector(0, 1);
            var midState = new ParticleState(midVelocity, ParticleType.Enemy);
            GameRoot.ParticleManager.CreateParticle(Art.LineParticle, position, whiteColor, duration, scale, midState);
            GameRoot.ParticleManager.CreateParticle(Art.Glow, position, midColor * alpha, duration, scale, midState);

            // side particle stream
            var velocity1 = baseVelocity + perpendicularVelocity + _random.NextVector(0, 0.3f);
            var velocity2 = baseVelocity - perpendicularVelocity + _random.NextVector(0, 0.3f);
            var state1 = new ParticleState(velocity1, ParticleType.Enemy);
            var state2 = new ParticleState(velocity2, ParticleType.Enemy);
            GameRoot.ParticleManager.CreateParticle(Art.LineParticle, position, whiteColor, duration, scale, state1);
            GameRoot.ParticleManager.CreateParticle(Art.LineParticle, position, whiteColor, duration, scale, state2);

            GameRoot.ParticleManager.CreateParticle(Art.Glow, position, sideColor, duration, scale, state1);
            GameRoot.ParticleManager.CreateParticle(Art.Glow, position, sideColor, duration, scale, state2);
        }
    }
}