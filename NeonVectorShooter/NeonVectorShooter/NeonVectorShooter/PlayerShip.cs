using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
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

        public override void Update()
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

            UpdatePosition();
            UpdateFire();
        }

        private void UpdatePosition()
        {
            const float speed = 8;
            Velocity = speed * Input.GetMovimentDirection();
            Position += Velocity;
            var halfSize = Size / 2;
            Position = Vector2.Clamp(Position, halfSize, GameRoot.ScreenSize - halfSize);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
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

                var offset = Vector2.Transform(new Vector2(25, -8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, velocity));

                offset = Vector2.Transform(new Vector2(25, 8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, velocity));
            }

            if (_cooldownRemaining > 0)
                _cooldownRemaining--;
        }

        public void Kill()
        {
            _framesUntilRespaw = PlayerStatus.IsGameOver ? 300 : 120;
            PlayerStatus.RemoveLife();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }
    }
}