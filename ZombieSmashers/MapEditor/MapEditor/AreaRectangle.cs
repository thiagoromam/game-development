using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLib;

namespace MapEditor
{
    public class AreaRectangle
    {
        public readonly Rectangle Area;
        private readonly Color _color;
        private readonly Rectangle _left;
        private readonly Rectangle _top;
        private readonly Rectangle _right;
        private readonly Rectangle _bottom;

        public AreaRectangle(int x, int y, int width, int height, Color color)
        {
            Area = new Rectangle(x, y, width, height);
            _color = color;

            _left = new Rectangle(Area.X, Area.Y, 1, Area.Height);
            _top = new Rectangle(Area.X, Area.Y, Area.Width, 1);
            _right = new Rectangle(Area.X + Area.Width, Area.Y, 1, Area.Height);
            _bottom = new Rectangle(Area.X, Area.Y + Area.Height, Area.Width, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(SharedArt.Null, _left, _color);
            spriteBatch.Draw(SharedArt.Null, _top, _color);
            spriteBatch.Draw(SharedArt.Null, _right, _color);
            spriteBatch.Draw(SharedArt.Null, _bottom, _color);
            spriteBatch.End();
        }
    }
}