using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public static class Art
    {
        public static SpriteFont Arial { get; private set; }
        public static Texture2D Null { get; private set; }
        public static Texture2D[] Maps { get; private set; }
        public static Texture2D Icons { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Arial = content.Load<SpriteFont>("Text/Arial");
            Null = content.Load<Texture2D>("Images/1x1");
            Icons = content.Load<Texture2D>("Images/Editor/icons");
            LoadMaps(content);
        }

        private static void LoadMaps(ContentManager content)
        {
            Maps = new Texture2D[1];
            for (var i = 0; i < Maps.Length; i++)
                Maps[i] = content.Load<Texture2D>("Images/Maps/maps" + (i + 1));
        }
    }
}
