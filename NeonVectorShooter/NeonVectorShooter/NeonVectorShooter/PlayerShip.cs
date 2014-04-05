using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter
{
    public class PlayerShip : Entity
    {
        private const int CooldownFrames = 6;
        private int _cooldownRemaining;
        private Random _random;

        private static PlayerShip _instance;
        public static PlayerShip Instance
        {
            get { return _instance ?? (_instance = new PlayerShip()); }
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
            UpdatePosition();
            UpdateFire();
        }

        private void UpdatePosition()
        {
            const float speed = 8;
            Velocity = speed*Input.GetMovimentDirection();
            Position += Velocity;
            var halfSize = Size/2;
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
    }
}