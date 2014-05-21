using System;
using System.Collections.Generic;
using System.Linq;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api.Controls;
using Microsoft.Xna.Framework;
using MouseLib.Api;
using TextLib.Api;

namespace GraphicalUserInterfaceLib.Controls
{
    public partial class TextButtonList<T> : ITextButtonList
    {
        private readonly List<TextButtonOption> _options;
        private readonly IMouseInput _mouseInput;
        private readonly IText _text;
        private int _currentIndex;
        private T _currentValue;
        private int? _hover;
        public Action<T> Change;

        public TextButtonList()
        {
            _text = DependencyInjection.Resolve<IText>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _options = new List<TextButtonOption>();
        }

        public TextButtonOption AddOption(T value, string text, Vector2 position)
        {
            var option = new TextButtonOption(value, text, position);
            _options.Add(option);
            return option;
        }
        public TextButtonOption AddOption(Vector2 position)
        {
            var option = new TextButtonOption(position);
            _options.Add(option);
            return option;
        }

        public T Value
        {
            set
            {
                _currentValue = value;
                _currentIndex = _options.IndexOf(_options.Single(o => Equals(o.Value, value)));
            }
        }

        public void Update()
        {
            _hover = null;

            for (var i = 0; i < _options.Count; i++)
            {
                if (i == _currentIndex) continue;

                var option = _options[i];
                if (_text.MouseIntersects(option.Text, option.Position))
                {
                    _hover = i;
                    if (!_mouseInput.LeftButtonClick) continue;

                    _currentIndex = i;

                    if (Change != null)
                        Change(_options[_currentIndex].Value);
                }
            }
        }

        public void Draw()
        {
            for (var i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                _text.Draw(option.Text, option.Position, i == _currentIndex ? Color.Lime : _hover == i ? Color.Yellow : Color.White);
            }
        }
    }
}