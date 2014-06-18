using Microsoft.Xna.Framework;

namespace ZombieSmashers.MapClasses
{
    public class BucketItem
    {
        public Vector2 Location;
        public int CharDef;

        public BucketItem(Vector2 loc, int charDef)
        {
            Location = loc;
            CharDef = charDef;
        }
    }
}