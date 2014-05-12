using Microsoft.Xna.Framework;

namespace MapEditor.Ioc.Api.Input
{
    public interface IMouseInput
    {
        bool LeftButtonClick { get; }
        bool LeftButtonDown { get; }
        bool LeftButtonPressed { get; }
        bool MiddleButtonPressed { get; }
        bool RightButtonClick { get; set; }
        Vector2 Position { get; }
        Vector2 PreviousPosition { get; }
    }
}