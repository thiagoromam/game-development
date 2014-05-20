using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public static class Art
    {
        public static Texture2D[] Maps { get; private set; }

        public static void LoadContent(ContentManager content)
        {
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
