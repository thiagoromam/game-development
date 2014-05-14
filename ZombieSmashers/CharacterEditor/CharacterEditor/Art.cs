using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor
{
    public static class Art
    {
        public static SpriteFont Font { get; private set; }
        public static Texture2D[] Heads { get; private set; }
        public static Texture2D[] Torsos { get; private set; }
        public static Texture2D[] Legs { get; private set; }
        public static Texture2D[] Weapons { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Font = content.Load<SpriteFont>("Text/Arial");

            Heads = new Texture2D[1];
            Torsos = new Texture2D[1];
            Legs = new Texture2D[1];
            Weapons = new Texture2D[1];

            LoadTextures(content, Heads, "Images/Character/head");
            LoadTextures(content, Torsos, "Images/Character/torso");
            LoadTextures(content, Legs, "Images/Character/legs");
            LoadTextures(content, Weapons, "Images/Character/weapon");
        }

        private static void LoadTextures(ContentManager content, Texture2D[] textures, string partialPath)
        {
            for (var i = 0; i < textures.Length; i++)
                textures[i] = content.Load<Texture2D>(partialPath + (i + 1));
        }
    }
}