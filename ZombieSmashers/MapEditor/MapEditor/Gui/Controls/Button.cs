using MapEditor.Helpers;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.Gui.Controls
{
    public class Button : IControl, IControlComponent
    {
        private readonly Rectangle _source;
        private readonly Rectangle _destination;
        private readonly Rectangle _destinationHover;
        private readonly IMouseInput _mouseInput;
        private bool _hover;

        public Button(int index, int x, int y)
        {
            _source = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            _destination = new Rectangle(x, y, 32, 32);
            _destinationHover = new Rectangle(x - 1, y - 1, 34, 34);

            _mouseInput = App.Container.Resolve<IMouseInput>();
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
            spriteBatch.Draw(Art.Icons, _hover ? _destinationHover : _destination, _source, Color.White);
            spriteBatch.End();
        }
    }
}