using System.Collections.Generic;
using System.Linq;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api.Controls;
using Microsoft.Xna.Framework;
using MouseLib.Api;
using TextLib.Api;

namespace GraphicalUserInterfaceLib.Controls
{
    public partial class TextButtonList<T> : ITextButtonList where T : struct
    {
        public delegate void ChangeHandler(T? previousValue, T? newValue);

        private readonly List<TextButtonOption> _options;
        private readonly IMouseInput _mouseInput;
        private readonly IText _text;
        private int? _currentIndex;
        private int? _hover;
        public ChangeHandler Change;

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

        public T? SelectedValue
        {
            get
            {
                if (_currentIndex.HasValue)
                    return _options[_currentIndex.Value].Value;

                return null;
            }
            set { _currentIndex = _options.IndexOf(_options.Single(o => Equals(o.Value, value))); }
        }
        public TextButtonOption SelectedOption
        {
            get { return _currentIndex.HasValue ? _options[_currentIndex.Value] : null; }
        }

        public void ClearValue()
        {
            if (!_currentIndex.HasValue)
                return;

            var currentValue = SelectedValue;
            _currentIndex = null;

            if (Change != null)
                Change(currentValue, null);
        }
        public void Update()
        {
            _hover = null;

            for (var i = 0; i < _options.Count; i++)
            {
                if (i == _currentIndex)
                    continue;

                var option = _options[i];
                if (!_text.MouseIntersects(option.Text, option.Position))
                    continue;

                _hover = i;
                if (!_mouseInput.LeftButtonClick)
                    continue;

                var previousOption = SelectedOption;
                _currentIndex = i;

                if (Change == null)
                    continue;

                T? previousValue;
                if (previousOption != null)
                    previousValue = previousOption.Value;
                else
                    previousValue = null;

                Change(previousValue, SelectedOption.Value);
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