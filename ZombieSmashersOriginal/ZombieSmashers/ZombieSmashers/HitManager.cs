using Microsoft.Xna.Framework;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Particles;

namespace ZombieSmashers
{
    public class HitManager
    {
        public static bool CheckHit(Particle p, Character[] c, ParticleManager pMan)
        {
            var r = false;
            var tFace = GetFaceFromTraj(p.Trajectory);
            for (var i = 0; i < c.Length; i++)
            {
                if (i != p.Owner)
                {
                    if (c[i] != null)
                    {
                        if (c[i].InHitBounds(p.Location))
                        {
                            if (p is Bullet)
                            {
                                c[i].Face = tFace == CharDir.Left ? CharDir.Right : CharDir.Left;
                                c[i].SetAnim("idle");
                                c[i].SetAnim("hit");
                                c[i].Slide(-100f);
                                pMan.MakeBulletBlood(p.Location, p.Trajectory/2f);
                                pMan.MakeBulletBlood(p.Location, -p.Trajectory);
                                pMan.MakeBulletDust(p.Location, p.Trajectory);
                                r = true;
                            }
                        }
                    }
                }
            }
            return r;
        }

        public static CharDir GetFaceFromTraj(Vector2 trajectory)
        {
            return (trajectory.X <= 0) ? CharDir.Left : CharDir.Right;
        }
    }
}