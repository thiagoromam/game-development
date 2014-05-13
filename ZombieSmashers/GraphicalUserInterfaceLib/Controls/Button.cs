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
        private readonly Rectangle _destination;
        private readonly Rectangle _destinationHover;
        private readonly IMouseInput _mouseInput;
        private bool _hover;

        public Button(Texture2D texture, Rectangle source, int x, int y)
        {
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

        public bool Clicked { get; private set; }

        public virtual void Update()
        {
            _hover = _destination.Contains(_mouseInput.Position);
            Clicked = _hover && _mouseInput.LeftButtonClick;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _hover ? _destinationHover : _destination, _source, Color.White);
            spriteBatch.End();
        }
    }
}