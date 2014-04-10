using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NeonVectorShooter
{
    public static class Art
    {
        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("Arts/Player");
            Seeker = content.Load<Texture2D>("Arts/Seeker");
            Wanderer = content.Load<Texture2D>("Arts/Wanderer");
            Bullet = content.Load<Texture2D>("Arts/Bullet");
            Pointer = content.Load<Texture2D>("Arts/Pointer");
            Font = content.Load<SpriteFont>("Arts/Font");
        }
    }
}