using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextLib
{
    public class Text
    {
        private readonly SpriteBatch _sprite;
        private readonly SpriteFont _font;
        private float _size = 1f;
        private Color _color = Color.White;

        public Text(SpriteBatch sprite, SpriteFont font)
        {
            _sprite = sprite;
            _font = font;
        }

        public void DrawText(int x, int y, String s)
        {
            _sprite.Begin();
            _sprite.DrawString(_font, s, new Vector2(
                x, y), _color, 0f, new Vector2(),
                _size, SpriteEffects.None, 1f);
            _sprite.End();
        }

        public bool DrawClickText(int x, int y, String s,
            int mosX, int mosY, bool mouseClick)
        {
            _color = Color.White;

            var r = false;

            if (mosX > x && mosY > y &&
                mosX < x + _font.MeasureString(s).X*_size &&
                mosY < y + _font.MeasureString(s).Y*_size)
            {
                _color = Color.Yellow;
                if (mouseClick)
                    r = true;
            }

            DrawText(x, y, s);

            return r;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}