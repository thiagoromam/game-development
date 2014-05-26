using Microsoft.Xna.Framework.Input;

namespace KeyboardLib.Api
{
    public interface IKeyboardControl
    {
        bool EditingMode { get; }
        void Focus(ITextEditor textEditor);
        bool IsKeyPressed(Keys key);
    }
}