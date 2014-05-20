using System;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api.Controls;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;

namespace GraphicalUserInterfaceLib.Controls
{
    public class Button : IButton
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _source;
        private readonly IMouseInput _mouseInput;
        private Rectangle _destinationHover;
        private Rectangle _destination;
        private Vector2 _position;
        public Action Click;
        private bool _hover;

        public Button(Texture2D texture, Rectangle source, int x, int y)
        {
            _position = new Vector2(x, y);
            _texture = texture;
            _source = source;
            _destination = new Rectangle(x, y, source.Width, source.Height);
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();

            _destinationHover = _destination;
            _destinationHover.X -= 1;
            _destinationHover.Y -= 1;
            _destinationHover.Width += 2;
            _destinationHover.Height += 2;
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _destination.X = (int) value.X;
                _destination.Y = (int) value.Y;
                _destinationHover.X = _destination.X - 1;
                _destinationHover.Y = _destination.Y - 1;
            }
        }

        public void Update()
        {
            _hover = _destination.Contains(_mouseInput.Position);
            if (_hover && _mouseInput.LeftButtonClick && Click != null)
                Click();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _hover ? _destinationHover : _destination, _source, Color.White);
            spriteBatch.End();
        }
    }
}