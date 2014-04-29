using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    public class MouseControl
    {
        public bool MouseClick { get; private set; }
        public bool RightMouseDown { get; private set; }
        public Vector2 Position { get; private set; }

        public void Update()
        {
            var state = Mouse.GetState();
            var previousMouseDown = RightMouseDown;

            Position = new Vector2(state.X, state.Y);
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