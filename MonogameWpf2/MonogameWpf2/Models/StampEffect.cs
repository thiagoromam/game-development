using Microsoft.Xna.Framework.Graphics;

namespace MonogameWpf2.Models
{
    public class StampEffect
    {
        public StampEffect(string name, Effect effect)
        {
            Name = name;
            Effect = effect;
        }

        public string Name { get; set; }
        public Effect Effect { get; set; }
    }
}