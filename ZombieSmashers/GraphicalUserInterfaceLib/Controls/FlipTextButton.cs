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
    public partial class FlipTextButton<T> : IFlipTextButton
    {
        private readonly List<FlipTextOption> _options;
        private readonly IText _text;
        private readonly IMouseInput _mouseInput;
        private bool _hover;
        public Vector2 Position;
        public Action<T> Change;
        private int _current;

        public FlipTextButton(int x, int y)
        {
            _text = DependencyInjection.Resolve<IText>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            Position = new Vector2(x, y);
            _options = new List<FlipTextOption>();
        }

        public void AddOption(T value, string text)
        {
            _options.Add(new FlipTextOption(value, text));
        }

        public T Value
        {
            set { _current = _options.IndexOf(_options.Single(o => Equals(o.Value, value))); }
        }

        public void Update()
        {
            var option = _options[_current];

            _hover = _text.MouseIntersects(option.Text, Position);
            var clicked = _hover && _mouseInput.LeftButtonClick;
            if (!clicked) return;

            _current = (_current + 1) % _options.Count;

            if (Change != null)
                Change(_options[_current].Value);
        }

        public void Draw()
        {
            _text.Draw(_options[_current].Text, Position, _hover ? Color.Yellow : Color.White);
        }
    }
}