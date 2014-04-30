using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    public class MouseControl
    {
        private Vector2 _position;
        private Vector2 _previousPosition;

        public bool MouseClick { get; private set; }
        public bool RightMouseDown { get; private set; }
        public Vector2 Position
        {
            get { return _position; }
        }
        public Vector2 PreviousPosition
        {
            get { return _previousPosition; }
        }

        public void Update()
        {
            _previousPosition = _position;

            var state = Mouse.GetState();
            var previousMouseDown = RightMouseDown;

            _position.X = state.X;
            _position.Y = state.Y;
            RightMouseDown = state.LeftButton == ButtonState.Pressed;
            MouseClick = previousMouseDown && !RightMouseDown;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Art.Icons, Position, new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}