using System;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api.Controls;
using Microsoft.Xna.Framework;
using MouseLib.Api;
using TextLib.Api;

namespace GraphicalUserInterfaceLib.Controls
{
    public class TextButton : ITextButton
    {
        private readonly IText _guiText;
        private readonly IMouseInput _mouseInput;
        public Vector2 Position;
        public Action Click;
        public string Text;
        private bool _hover;

        protected TextButton(string text, int x, int y)
        {
            _guiText = DependencyInjection.Resolve<IText>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();

            Text = text;
            Position = new Vector2(x, y);
        }

        public void Update()
        {
            _hover = _guiText.MouseIntersects(Text, Position);
            if (_hover && _mouseInput.LeftButtonClick && Click != null)
                Click();
        }

        public void Draw()
        {
            _guiText.Draw(Text, Position, _hover ? Color.Lime : Color.White);
        }
    }
}