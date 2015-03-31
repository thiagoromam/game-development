using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;

namespace MonogameWpf2.Util
{
    public static class GameControlHelper
    {
        public static Vector2 GetPosition(MouseEventArgs e, IInputElement relativeTo)
        {
            var position = e.GetPosition(relativeTo);
            return new Vector2((float)position.X, (float)position.Y);
        }
    }
}