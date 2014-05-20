using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharedLib
{
    public static class SharedArt
    {
        public static SpriteFont Arial { get; private set; }
        public static Texture2D Null { get; private set; }
        public static Texture2D Icons { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Arial = content.Load<SpriteFont>("Text/Arial");
            Icons = content.Load<Texture2D>("Images/Editor/icons");
            Null = content.Load<Texture2D>("Images/1x1");
        }
    }
}
