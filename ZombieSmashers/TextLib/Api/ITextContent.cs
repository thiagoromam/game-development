using Microsoft.Xna.Framework.Graphics;

namespace TextLib.Api
{
    public interface ITextContent
    {
        SpriteFont Font { get; set; }
        float Size { get; set; }
    }
}