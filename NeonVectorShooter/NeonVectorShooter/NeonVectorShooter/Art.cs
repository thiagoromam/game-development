using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
{
    public static class Art
    {
        public static Texture2D Player { get; set; }
        public static Texture2D Seeker { get; set; }
        public static Texture2D Wanderer { get; set; }
        public static Texture2D Bullet { get; set; }
        public static Texture2D Pointer { get; set; }

        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("Player");
            Seeker = content.Load<Texture2D>("Seeker");
            Wanderer = content.Load<Texture2D>("Wanderer");
            Bullet = content.Load<Texture2D>("Bullet");
            Pointer = content.Load<Texture2D>("Pointer");
        }
    }
}