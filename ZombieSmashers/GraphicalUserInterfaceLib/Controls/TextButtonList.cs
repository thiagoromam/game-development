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
        public delegate void ChangeHandler(T previousValue, T newValue);
        
        private readonly List<TextButtonOption> _options;
        private readonly IMouseInput _mouseInput;
        private readonly IText _text;
        private int _currentIndex;
        private int? _hover;
        public ChangeHandler Change; 
        private T _currentValue;

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

        public T Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                _currentIndex = _options.IndexOf(_options.Single(o => Equals(o.Value, value)));
            }
        }

        public virtual void Update()
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

                    var previousValue = _currentValue;
                    _currentIndex = i;
                    _currentValue = _options[_currentIndex].Value; 

                    if (Change != null)
                        Change(previousValue, _currentValue);
                }
            }
        }

        public virtual void Draw()
        {
            for (var i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                _text.Draw(option.Text, option.Position, i == _currentIndex ? Color.Lime : _hover == i ? Color.Yellow : Color.White);
            }
        }
    }
}