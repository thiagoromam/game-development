using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Ais
{
    public class Zombie : Ai
    {
        public override void Update(Character[] c, int id, Map map)
        {
            Me = c[id];
            if (JobFrame < 0f)
            {
                var r = Rand.GetRandomFloat(0f, 1f);
                if (r < 0.6f)
                {
                    Job = JobMeleeChase;
                    JobFrame = Rand.GetRandomFloat(2f, 4f);
                    Targ = FindTarg(c);
                }
                else if (r < 0.8f)
                {
                    Job = JobAvoid;
                    JobFrame = Rand.GetRandomFloat(1f, 2f);
                    Targ = FindTarg(c);
                }
                else
                {
                    Job = JobIdle;
                    JobFrame = Rand.GetRandomFloat(.5f, 1f);
                }
            }

            base.Update(c, id, map);
        }
    }
}