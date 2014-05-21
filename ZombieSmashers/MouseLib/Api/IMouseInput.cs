using Microsoft.Xna.Framework;

namespace MouseLib.Api
{
    public interface IMouseInput
    {
        bool LeftButtonClick { get; }
        bool LeftButtonDown { get; }
        bool LeftButtonPressed { get; }
        bool MiddleButtonPressed { get; }
        bool RightButtonClick { get; }
        bool RightButtonPressed { get; }
        Vector2 Position { get; }
        Vector2 PreviousPosition { get; }
    }
}