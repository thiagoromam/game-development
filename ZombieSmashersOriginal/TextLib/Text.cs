using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TextLib
{
    public class Text
    {
        SpriteBatch sprite;
        SpriteFont font;
        float size = 1f;
        Color color = Color.White;

        public Text(SpriteBatch _sprite, SpriteFont _font)
        {
            sprite = _sprite;
            font = _font;
        }

        public void DrawText(int x, int y, String s)
        {
            sprite.Begin();
            sprite.DrawString(font, s, new Vector2(
                (float)x, (float)y), color, 0f, new Vector2(),
                size, SpriteEffects.None, 1f);
            sprite.End();
        }

        public bool DrawClickText(int x, int y, String s,
            int mosX, int mosY, bool mouseClick)
        {
            color = Color.White;

            bool r = false;

            if (mosX > x && mosY > y &&
                mosX < x + font.MeasureString(s).X * size &&
                mosY < y + font.MeasureString(s).Y * size)
            {
                color = Color.Yellow;
                if (mouseClick)
                    r = true;
            }

            DrawText(x, y, s);

            return r;
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }
    }
}
