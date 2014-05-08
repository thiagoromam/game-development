using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;

namespace MapEditor.Gui.Controls
{
    public interface IFlipTextButton : IControlComponent, ITextControl { }

    public partial class FlipTextButton<T> : IFlipTextButton
    {
        private readonly List<FlipTextOption> _options;
        private readonly IMouseInput _mouseInput;
        private readonly IGuiText _text;
        private readonly Vector2 _position;
        private bool _hover;
        public Action<T> Change;
        private int _current;

        public FlipTextButton(int x, int y)
        {
            _position = new Vector2(x, y);
            _mouseInput = App.Container.Resolve<IMouseInput>();
            _text = App.Container.Resolve<IGuiText>();
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

            _hover = _text.MouseIntersects(option.Text, _position);
            var clicked = _hover && _mouseInput.LeftButtonClick;
            if (!clicked) return;

            _current = (_current + 1) % _options.Count;

            if (Change != null)
                Change(_options[_current].Value);
        }

        public void Draw()
        {
            _text.Draw(_options[_current].Text, _position, _hover ? Color.Yellow : Color.White);
        }
    }
}