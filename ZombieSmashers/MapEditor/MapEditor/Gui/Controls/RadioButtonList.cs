using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;

namespace MapEditor.Gui.Controls
{
    public partial class RadioButtonList<T> : IControlComponent, ITextControl
    {
        private readonly List<RadioButtonOption> _options;
        private int _current;
        private int? _hover;
        public Action<T> Change;
        private readonly IGuiText _text;
        private readonly IMouseInput _mouseInput;

        public RadioButtonList()
        {
            _options = new List<RadioButtonOption>();
            _text = App.Container.Resolve<IGuiText>();
            _mouseInput = App.Container.Resolve<IMouseInput>();
        }

        public void AddOption(T value, string text, Vector2 position)
        {
            _options.Add(new RadioButtonOption(value, text, position));
        }

        public T Value
        {
            set { _current = _options.IndexOf(_options.Single(o => Equals(o.Value, value))); }
        }

        public void Update()
        {
            _hover = null;

            for (var i = 0; i < _options.Count; i++)
            {
                if (i == _current) continue;

                var option = _options[i];
                if (_text.MouseIntersects(option.Text, option.Position))
                {
                    _hover = i;
                    if (!_mouseInput.LeftButtonClick) continue;

                    _current = i;

                    if (Change != null)
                        Change(_options[_current].Value);
                }
            }
        }

        public void Draw()
        {
            for (var i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                _text.Draw(option.Text, option.Position, i == _current ? Color.Lime : _hover == i ? Color.Yellow : Color.White);
            }
        }
    }
}