using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameWpf2.Models
{
    public class Stamp
    {
        public Stamp()
        {
            CuttingEffectPercent = 0.5f;
        }

        public Effect Effect { get; set; }
        public Vector2 Position { get; set; }
        public float CuttingEffectPercent { get; set; }
    }
}