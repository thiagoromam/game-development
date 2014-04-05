using Microsoft.Xna.Framework;

namespace NeonVectorShooter
{
    public class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            Image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = velocity.ToAngle();
            Radius = 8;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}