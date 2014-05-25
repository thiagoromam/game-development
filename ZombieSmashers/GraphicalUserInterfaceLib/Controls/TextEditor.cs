using System;
using Funq.Fast;
using Microsoft.Xna.Framework;
using MouseLib.Api;
using TextLib.Api;
using ITextEditor = GraphicalUserInterfaceLib.Api.Controls.ITextEditor;
using IKeyboardControl = KeyboardLib.Api.IKeyboardControl;
using IKeyboardTextEditor = KeyboardLib.Api.ITextEditor;

namespace GraphicalUserInterfaceLib.Controls
{
    public class TextEditor : ITextEditor, IKeyboardTextEditor
    {
        private readonly IText _text;
        private readonly IMouseInput _mouseInput;
        private readonly IKeyboardControl _keyboardControl;
        private bool _editing;
        private bool _hover;
        private string _previousText;
        public Vector2 Position;
        public Action<string> Change;
        public bool Visible;

        public TextEditor(int x, int y)
        {
            _text = DependencyInjection.Resolve<IText>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _keyboardControl = DependencyInjection.Resolve<IKeyboardControl>();
            Position = new Vector2(x, y);
            Visible = true;
        }

        public string Text { get; set; }

        public void Update()
        {
            if (!Visible)
                return;

            _hover = !_editing && _text.MouseIntersects(Text, Position);
            if (_editing) return;

            var clicked = _hover && _mouseInput.LeftButtonClick;
            if (clicked)
            {
                _editing = true;
                _previousText = Text;
                _keyboardControl.Focus(this);
            }
        }

        public void Draw()
        {
            if (!Visible)
                return;

            var text = Text;
            if (_editing)
                text += "*";

            _text.Draw(text, Position, _editing ? Color.GreenYellow : _hover ? Color.Yellow : Color.White);
        }

        public void RemoveFocus()
        {
            _editing = false;

            if (Text != _previousText && Change != null)
                Change(Text);
        }
    }
}
