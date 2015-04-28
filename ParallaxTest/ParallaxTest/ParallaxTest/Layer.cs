using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParallaxTest
{
    public class Layer
    {
        private readonly Camera _camera;

        public Layer(Camera camera)
        {
            _camera = camera;
            Parallax = Vector2.One;
            Sprites = new List<Sprite>();
        }

        public Vector2 Parallax { get; set; }
        public List<Sprite> Sprites { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix(Parallax));
            foreach (Sprite sprite in Sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}