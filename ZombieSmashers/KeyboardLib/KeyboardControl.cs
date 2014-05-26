using KeyboardLib.Api;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace KeyboardLib
{
    internal class KeyboardControl : IKeyboardComponent, IKeyboardControl
    {
        private KeyboardState _lastState;
        private KeyboardState _state;
        private ITextEditor _focus;
        private string _textBackup;

        public bool EditingMode
        {
            get { return _focus != null; }
        }

        public void Focus(ITextEditor textEditor)
        {
            RemoveFocus();
            _focus = textEditor;
            _textBackup = _focus.Text;
        }
        private void RemoveFocus()
        {
            if (!EditingMode) return;

            _focus.RemoveFocus();
            _focus = null;
            _textBackup = null;
            _lastState = new KeyboardState();
        }

        public void Update()
        {
            _lastState = _state;
            _state = Keyboard.GetState();

            if (EditingMode)
                UpdateEditingMode();
        }

        private void UpdateEditingMode()
        {
            var lastKeys = _lastState.GetPressedKeys();

            if (lastKeys.Contains(Keys.Escape))
            {
                if (IsKeyPressed(Keys.Escape))
                {
                    _focus.Text = _textBackup;
                    RemoveFocus();
                }

                return;
            }

            if (lastKeys.Contains(Keys.Enter))
            {
                if (IsKeyPressed(Keys.Enter))
                    RemoveFocus();
                
                return;
            }

            var currentKeys = _state.GetPressedKeys();
            foreach (var key in currentKeys)
            {
                if (key == Keys.Enter || key == Keys.Escape)
                    break;

                if (lastKeys.Contains(key))
                    break;

                EditingModeKeyPressed(key);
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return _lastState.IsKeyDown(key) && _state.IsKeyUp(key);
        }

        private void EditingModeKeyPressed(Keys key)
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
