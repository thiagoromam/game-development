using KeyboardLib.Api;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace KeyboardLib
{
    internal class KeyboardControl : IKeyboardComponent, IKeyboardControl
    {
        private KeyboardState _lastState;
        private ITextEditor _focus;

        public void Focus(ITextEditor textEditor)
        {
            RemoveFocus();
            _focus = textEditor;
        }

        private void RemoveFocus()
        {
            if (_focus == null) return;

            _focus.RemoveFocus();
            _focus = null;
            _lastState = new KeyboardState();
        }

        public void Update()
        {
            if (_focus == null) return;

            var state = Keyboard.GetState();

            var currentKeys = state.GetPressedKeys();
            var lastKeys = _lastState.GetPressedKeys();

            foreach (var key in currentKeys)
            {
                if (lastKeys.Contains(key)) continue;

                if (key == Keys.Enter)
                {
                    RemoveFocus();
                    break;
                }

                KeyPressed(key);
            }

            _lastState = state;
        }

        private void KeyPressed(Keys key)
        {
            var value = _focus.Text ?? string.Empty;

            switch (key)
            {
                case Keys.Back:
                    if (value.Length > 0)
                        value = value.Remove(value.Length - 1);
                    break;
                default:
                    var charKey = key == Keys.OemPeriod ? '.' : (char)key;
                    value = (value + charKey).ToLower();
                    break;
            }

            _focus.Text = value;
        }
    }
}
