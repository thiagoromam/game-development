using Microsoft.Xna.Framework;
using ZombieSmashers.Audio;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Particles;
using ZombieSmashers.Shakes;

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
                                #region Bullet

                                c[i].Face = tFace == CharDir.Left ? CharDir.Right : CharDir.Left;
                                c[i].SetAnim("idle");
                                c[i].SetAnim("hit");
                                c[i].Slide(-100f);
                                Sound.PlayCue("bullethit");
                                pMan.MakeBulletBlood(p.Location, p.Trajectory / 2f);
                                pMan.MakeBulletBlood(p.Location, -p.Trajectory);
                                pMan.MakeBulletDust(p.Location, p.Trajectory);
                                r = true;

                                #endregion
                            }
                            else if (p is Hit)
                            {
                                #region Hit

                                c[i].Face = (tFace == CharDir.Left) ? CharDir.Right : CharDir.Left;
                                var tX = 1f;
                                if (tFace == CharDir.Left)
                                    tX = -1f;
                                c[i].SetAnim("idle");
                                c[i].SetAnim("hit");
                                Sound.PlayCue("zomhit");

                                if (c[i].State == CharState.Grounded)
                                    c[i].Slide(-200f);
                                else
                                    c[i].Slide(-50f);

                                switch (p.Flag)
                                {
                                    case Character.TrigWrenchDiagDown:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(50f * tX, 100f));
                                        Game1.SlowTime = 0.1f;
                                        break;
                                    case Character.TrigWrenchDiagUp:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(-50f * tX, -100f));
                                        Game1.SlowTime = 0.1f;
                                        break;
                                    case Character.TrigWrenchUp:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(30f * tX, -100f));
                                        Game1.SlowTime = 0.1f;
                                        break;
                                    case Character.TrigWrenchDown:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(-50f * tX, 100f));
                                        Game1.SlowTime = 0.1f;
                                        break;
                                    case Character.TrigWrenchUppercut:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(-50f * tX, -150f));
                                        c[i].Trajectory.X = 100f * tX;
                                        c[i].SetAnim("jhit");
                                        c[i].SetJump(700f);
                                        Game1.SlowTime = 0.125f;
                                        QuakeManager.SetQuake(.5f);
                                        QuakeManager.SetBlast(.5f, p.Location);
                                        break;
                                    case Character.TrigWrenchSmackdown:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(-50f * tX, 150f));
                                        c[i].SetAnim("jfall");
                                        c[i].SetJump(-900f);
                                        Game1.SlowTime = 0.125f;
                                        break;
                                    case Character.TrigKick:
                                        pMan.MakeBloodSplash(p.Location, new Vector2(300f * tX, 0f));
                                        c[i].Trajectory.X = 1000f * tX;
                                        c[i].SetAnim("jhit");
                                        c[i].SetJump(300f);
                                        Game1.SlowTime = 0.25f;
                                        break;
                                }

                                #endregion
                            }

                            if (c[i].State == CharState.Air)
                            {
                                if (c[i].AnimName == "hit")
                                {
                                    c[i].SetAnim("jmid");
                                    c[i].SetJump(300f);
                                    if (p is Hit)
                                    {
                                        if (c[p.Owner].Team == Character.TeamGoodGuys)
                                            c[i].Location.Y = c[p.Owner].Location.Y;
                                    }
                                }
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