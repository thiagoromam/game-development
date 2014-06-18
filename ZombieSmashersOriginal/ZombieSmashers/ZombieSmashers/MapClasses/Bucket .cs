using Microsoft.Xna.Framework;
using ZombieSmashers.CharClasses;

namespace ZombieSmashers.MapClasses
{
    public class Bucket
    {
        private readonly BucketItem[] bucketItem;
        public int Size;
        private readonly Map _map;
        private float updateFrame = 0f;
        public bool IsEmpty = false;

        public Bucket(int size, Map map)
        {
            bucketItem = new BucketItem[64];
            Size = size;
            _map = map;

            for (var i = 0; i < bucketItem.Length; i++)
                bucketItem[i] = null;
        }

        public void AddItem(Vector2 loc, int charDef)
        {
            for (var i = 0; i < bucketItem.Length; i++)
            {
                if (bucketItem[i] == null)
                {
                    bucketItem[i] = new BucketItem(loc, charDef);
                    return;
                }
            }
        }

        public void Update(Character[] c)
        {
            updateFrame -= Game1.FrameTime;

            if (updateFrame > 0f)
                return;

            updateFrame = 1f;
            var monsters = 0;

            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] != null)
                    if (c[i].Team == Character.TeamBadGuys) monsters++;
            }

            if (monsters < Size)
            {
                for (var i = 0; i < bucketItem.Length; i++)
                {
                    if (bucketItem[i] != null)
                    {
                        for (var n = 0; n < c.Length; n++)
                        {
                            if (c[n] == null)
                            {
                                c[n] = new Character(bucketItem[i].Location, Game1.CharDefs[bucketItem[i].CharDef], n,
                                    Character.TeamBadGuys) { Map = _map };
                                bucketItem[i] = null;
                                return;
                            }
                        }
                    }
                }

                if (monsters == 0)
                    IsEmpty = true;
            }
        }
    }
}