using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieSmashers.Gui
{
    public class ScoreDraw
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spritesTex;

        public ScoreDraw(SpriteBatch spriteBatch, Texture2D spritesTex)
        {
            _spriteBatch = spriteBatch;
            _spritesTex = spritesTex;
        }

        public void Draw(long score, Vector2 loc, Color color, Justification justify)
        {
            var place = 0;

            if (justify == Justification.Left)
            {
                loc.X -= 17f;
                var s = score;
                if (s == 0)
                {
                    loc.X += 17f;
                }
                else
                {
                    while (s > 0)
                    {
                        s /= 10;
                        loc.X += 17f;
                    }
                }
            }

            while (true)
            {
                var digit = score % 10;
                score = score / 10;

                _spriteBatch.Draw(_spritesTex, loc + new Vector2(place * -17f, 0f),
                    new Rectangle((int)digit * 16, 224, 16, 32), color);
                place++;

                if (score <= 0)
                    return;
            }
        }
    }
}