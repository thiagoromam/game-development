using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace MapEditor.Editor.Controls
{
    public class DefaultButton : Button
    {
        public DefaultButton(int index, int x, int y)
            : base(Art.Icons, CalculateSource(index), x, y)
        {
        }

        private static Rectangle CalculateSource(int index)
        {
            return new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
        }
    }
}