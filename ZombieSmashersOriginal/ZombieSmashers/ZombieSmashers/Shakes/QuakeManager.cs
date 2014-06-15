using Microsoft.Xna.Framework;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers.Shakes
{
    public class QuakeManager
    {
        public static Rumble[] Rumbles = new Rumble[4];
        public static Quake Quake;
        public static Blast Blast;

        static QuakeManager()
        {
            Quake = new Quake();
            Blast = new Blast();
            for (var i = 0; i < Rumbles.Length; i++)
                Rumbles[i] = new Rumble(i);
        }

        public static void Update()
        {
            Quake.Update();
            Blast.Update();
            for (var i = 0; i < Rumbles.Length; i++)
                Rumbles[i].Update();
        }

        public static void SetBlast(float mag, Vector2 center)
        {
            Blast.Value = mag;
            Blast.Magnitude = mag;
            Blast.Center = center;
        }

        public static void SetQuake(float val)
        {
            if (Quake.Value < val) Quake.Value = val;
            for (var i = 0; i < Rumbles.Length; i++)
            {
                Rumbles[i].Left = val;
                Rumbles[i].Right = val;
            }
        }

        public static void SetRumble(int i, int motor, float val)
        {
            if (motor == 0)
                Rumbles[i].Left = val;
            else
                Rumbles[i].Right = val;
        }
    }
}