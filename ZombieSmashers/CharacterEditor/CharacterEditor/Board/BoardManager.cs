using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Board
{
    public class BoardManager
    {
        private readonly CharacterAnimation _characterAnimation;
        private readonly CharacterBoard _characterBoard;

        public BoardManager()
        {
            _characterBoard = new CharacterBoard();
            _characterAnimation = new CharacterAnimation();
        }

        public void Update(GameTime gameTime)
        {
            _characterBoard.Update();
            _characterAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _characterBoard.Draw(spriteBatch);
            _characterAnimation.Draw(spriteBatch);
        }
    }
}