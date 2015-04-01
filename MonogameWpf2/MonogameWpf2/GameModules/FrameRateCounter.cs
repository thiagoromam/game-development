using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameWpf2.GameModules
{
    // https://github.com/alaskajohn/2dGPfG/tree/master/2dGPfG-XNA/9-1-2-PixelModBlur-GPU
    public class FrameRateCounter
    {
        int _mIFps;
        int _mIFrameCount;
        double _mIElapsedMilliseconds;
        SpriteFont _myFont;
        readonly Vector2 _renderLoc = new Vector2(50, 50);

        public void LoadContent(ContentManager content)
        {
            _myFont = content.Load<SpriteFont>("Fonts/font");
        }
        public void Update(GameTime gameTime)
        {
            _mIElapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_mIElapsedMilliseconds > 1000)
            {
                _mIElapsedMilliseconds -= 1000;
                _mIFps = _mIFrameCount;
                _mIFrameCount = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _mIFrameCount++;

            string fps = string.Format("{0} FPS", _mIFps);

            spriteBatch.Begin();
            spriteBatch.DrawString(_myFont, fps, _renderLoc, Color.Red);
            spriteBatch.End();
        }
    }
}