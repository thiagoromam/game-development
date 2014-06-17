using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Ais
{
    public class Ai
    {
        public const int JobIdle = 0;
        public const int JobMeleeChase = 1;
        public const int JobShootChase = 2;
        public const int JobAvoid = 3;
        protected int Job = JobIdle;
        protected int Targ = -1;
        protected float JobFrame = 0f;
        protected Character Me;

        public virtual void Update(Character[] c, int id, Map map)
        {
            Me = c[id];
            Me.KeyLeft = false;
            Me.KeyRight = false;
            Me.KeyUp = false;
            Me.KeyDown = false;
            Me.KeyAttack = false;
            Me.KeySecondary = false;
            Me.KeyJump = false;
            JobFrame -= Game1.FrameTime;
            DoJob(c, id);
        }

        protected void DoJob(Character[] c, int id)
        {
            switch (Job)
            {
                case JobIdle: //do nothing!
                    break;
                case JobMeleeChase:
                    if (Targ > -1)
                    {
                        if (!ChaseTarg(c, 50f))
                        {
                            if (!FaceTarg(c))
                            {
                                Me.KeyAttack = true;
                            }
                        }
                    }
                    else Targ = FindTarg(c);
                    break;
                case JobAvoid:
                    if (Targ > -1)
                    {
                        AvoidTarg(c, 500f);
                    }
                    else Targ = FindTarg(c);
                    break;
                case JobShootChase:
                    if (Targ > -1)
                    {
                        if (!ChaseTarg(c, 150f))
                        {
                            if (!FaceTarg(c))
                            {
                                Me.KeySecondary = true;
                            }
                        }
                    }
                    else Targ = FindTarg(c);
                    break;
            }

            if (!Me.KeyAttack && !Me.KeySecondary)
            {
                if (Me.KeyLeft)
                {
                    if (FriendInWay(c, id, CharDir.Left))
                        Me.KeyLeft = false;
                }
                if (Me.KeyRight)
                {
                    if (FriendInWay(c, id, CharDir.Right))
                        Me.KeyRight = false;
                }
            }
        }

        protected int FindTarg(Character[] c)
        {
            var closest = -1;
            var d = 0f;

            for (var i = 0; i < c.Length; i++)
            {
                if (i != Me.Id)
                {
                    if (c[i] != null)
                    {
                        if (c[i].Team != Me.Team)
                        {
                            var newD = (Me.Location - c[i].Location).Length();
                            if (closest == -1 || newD < d)
                            {
                                d = newD;
                                closest = i;
                            }
                        }
                    }
                }
            }

            return closest;
        }

        private bool FriendInWay(Character[] c, int id, CharDir face)
        {
            for (var i = 0; i < c.Length; i++)
            {
                if (i != id && c[i] != null)
                {
                    if (Me.Team == c[i].Team)
                    {
                        if (Me.Location.Y > c[i].Location.Y - 100f && Me.Location.Y < c[i].Location.Y + 10f)
                        {
                            if (face == CharDir.Right)
                            {
                                if (c[i].Location.X > Me.Location.X && c[i].Location.X < Me.Location.X + 70f)
                                    return true;
                            }
                            else
                            {
                                if (c[i].Location.X < Me.Location.X && c[i].Location.X > Me.Location.X - 70f)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected bool ChaseTarg(Character[] c, float distance)
        {
            if (Me.Location.X > c[Targ].Location.X + distance)
            {
                Me.KeyLeft = true;
                return true;
            }
            if (Me.Location.X < c[Targ].Location.X - distance)
            {
                Me.KeyRight = true;
                return true;
            }
            return false;
        }

        protected bool AvoidTarg(Character[] c, float distance)
        {
            if (Me.Location.X < c[Targ].Location.X + distance)
            {
                Me.KeyRight = true;
                return true;
            }
            if (Me.Location.X > c[Targ].Location.X - distance)
            {
                Me.KeyLeft = true;
                return true;
            }
            return false;
        }

        protected bool FaceTarg(Character[] c)
        {
            if (Me.Location.X > c[Targ].Location.X && Me.Face == CharDir.Right)
            {
                Me.KeyLeft = true;
                return true;
            }
            if (Me.Location.X < c[Targ].Location.X && Me.Face == CharDir.Left)
            {
                Me.KeyRight = true;
                return true;
            }
            return false;
        }
    }
}