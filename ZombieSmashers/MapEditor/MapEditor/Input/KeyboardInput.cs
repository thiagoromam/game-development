using MapEditor.Gui.Controls;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace MapEditor.Input
{
    public class KeyboardInput
    {
        private static TextEditor _focus;
        private static KeyboardState _lastState;

        public static void Focus(TextEditor textEditor)
        {
            _focus = textEditor;
        }

        public static void RemoveFocus()
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

        private static void KeyPressed(Keys key)
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