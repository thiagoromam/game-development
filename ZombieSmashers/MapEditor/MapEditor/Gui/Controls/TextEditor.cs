using System;
using MapEditor.Input;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;

namespace MapEditor.Gui.Controls
{
    public class TextEditor : IControlComponent, ITextControl
    {
        private readonly IGuiText _guiText;
        private readonly IMouseInput _mouseInput;
        private readonly Vector2 _position;
        private bool _editing;
        private bool _hover;
        private string _previousText;
        public string Text;
        public Action<string> Change;

        public TextEditor(int x, int y)
        {
            _guiText = App.Container.Resolve<IGuiText>();
            _mouseInput = App.Container.Resolve<IMouseInput>();
            _position = new Vector2(x, y);
        }

        public void Update()
        {
            _hover = !_editing && _guiText.MouseIntersects(Text, _position);
            if (_editing) return;

            var clicked = _hover && _mouseInput.LeftButtonClick;
            if (clicked)
            {
                _editing = true;
                _previousText = Text;
                KeyboardInput.Focus(this);
            }
        }

        public void Draw()
        {
            var text = Text;
            if (_editing)
                text += "*";

            _guiText.Draw(text, _position, _editing ? Color.GreenYellow : _hover ? Color.Yellow : Color.White);
        }

        public void RemoveFocus()
        {
            _editing = false;

            if (Text != _previousText && Change != null)
                Change(Text);
        }
    }
}
