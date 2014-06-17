using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.Ais;
using ZombieSmashers.Audio;
using ZombieSmashers.Input;
using ZombieSmashers.MapClasses;
using ZombieSmashers.Particles;
using ZombieSmashers.Shakes;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers.CharClasses
{
    public class Character
    {
        public static Texture2D[] HeadTex = new Texture2D[2];
        public static Texture2D[] TorsoTex = new Texture2D[2];
        public static Texture2D[] LegsTex = new Texture2D[2];
        public static Texture2D[] WeaponTex = new Texture2D[1];

        public static void LoadContent(ContentManager content)
        {
            for (var i = 0; i < HeadTex.Length; i++)
                HeadTex[i] = content.Load<Texture2D>("gfx/head" + (i + 1));

            for (var i = 0; i < TorsoTex.Length; i++)
                TorsoTex[i] = content.Load<Texture2D>("gfx/torso" + (i + 1));

            for (var i = 0; i < LegsTex.Length; i++)
                LegsTex[i] = content.Load<Texture2D>("gfx/legs" + (i + 1));

            for (var i = 0; i < WeaponTex.Length; i++)
                WeaponTex[i] = content.Load<Texture2D>("gfx/weapon" + (i + 1));
        }

        public const int TrigPistolAcross = 0;
        public const int TrigPistolUp = 1;
        public const int TrigPistolDown = 2;
        public const int TrigWrenchUp = 3;
        public const int TrigWrenchDown = 4;
        public const int TrigWrenchDiagUp = 5;
        public const int TrigWrenchDiagDown = 6;
        public const int TrigWrenchUppercut = 7;
        public const int TrigWrenchSmackdown = 8;
        public const int TrigKick = 9;
        public const int TrigZombieHit = 10;
        public const int TrigBloodSquirtUp = 11;
        public const int TrigBloodSquirtUpForward = 12;
        public const int TrigBloodSquirtForward = 13;
        public const int TrigBloodSquirtDownForward = 14;
        public const int TrigBloodSquirtDown = 15;
        public const int TrigBloodSquirtDownBack = 16;
        public const int TrigBloodSquirtBack = 17;
        public const int TrigBloodSquirtUpBack = 18;
        public const int TrigBloodCloud = 19;
        public const int TrigBloodSplat = 20;
        public const int TeamGoodGuys = 0;
        public const int TeamBadGuys = 1;

        // Animation fields
        public Vector2 Location;
        public Vector2 Trajectory;
        public CharDir Face;
        public float Scale;
        public int AnimFrame;
        public CharState State;
        public int Anim;
        public string AnimName;
        private float _frame;

        // Control and script fields
        public bool KeyLeft;
        public bool KeyRight;
        public bool KeyUp;
        public bool KeyDown;
        public bool KeyJump;
        public bool KeyAttack;
        public bool KeySecondary;
        private readonly CharDef _charDef;
        private int _ledgeAttach = -1;
        public bool Floating;
        public float Speed = 200;
        public int[] GotoGoal = { -1, -1, -1, -1, -1, -1, -1, -1 };
        public PressedKeys PressedKey;
        private readonly Script _script;
        public float ColMove;
        public Ai Ai;
        public bool Ethereal;
        public float DyingFrame = -1;

        public Map Map;
        public int Id;
        public int Team;
        public int Hp;
        public int Mhp;

        public Character(Vector2 newLoc, CharDef newCharDef, int newId, int newTeam)
        {
            Location = newLoc;
            Trajectory = Vector2.Zero;

            Face = CharDir.Right;
            Scale = 0.5f;
            _charDef = newCharDef;
            Id = newId;
            Team = newTeam;
            _script = new Script(this);

            Ai = null;
            InitScript();
            Ethereal = false;
            AnimName = "";
            State = CharState.Air;
            SetAnim("fly");
        }

        public CharDef Definition
        {
            get { return _charDef; }
        }

        public void SetAnim(string newAnim)
        {
            if (newAnim == AnimName)
                return;

            for (var i = 0; i < _charDef.Animations.Length; i++)
            {
                if (_charDef.Animations[i].Name == newAnim)
                {
                    Floating = false;
                    Anim = i;
                    AnimFrame = 0;
                    _frame = 0;
                    AnimName = newAnim;
                    Ethereal = false;

                    break;
                }
            }
        }

        public void Update(GameTime gameTime, ParticleManager pMan, Character[] c)
        {
            #region Update AI

            if (Ai != null)
                Ai.Update(c, Id, Map);

            #endregion

            #region Update Dying

            if (DyingFrame > -1)
                DyingFrame += Game1.FrameTime;

            #endregion

            #region Update animation

            if (DyingFrame < 0)
            {
                var animation = _charDef.Animations[Anim];
                var keyFrame = animation.KeyFrames[AnimFrame];

                _frame += Game1.FrameTime * 30;

                if (_frame > keyFrame.Duration)
                {
                    var pFrame = AnimFrame;
                    _script.DoScript(Anim, AnimFrame);
                    CheckTrig(pMan);

                    _frame -= keyFrame.Duration;

                    if (AnimFrame == pFrame)
                        AnimFrame++;

                    if (AnimFrame >= animation.KeyFrames.Length)
                        AnimFrame = 0;

                    keyFrame = animation.KeyFrames[AnimFrame];

                    if (keyFrame.FrameRef < 0)
                        AnimFrame = 0;
                }
            }

            #endregion

            #region Collison w/ other characters

            for (var i = 0; i < c.Length; i++)
            {
                if (i != Id)
                {
                    if (c[i] != null)
                    {
                        if (!Ethereal && !c[i].Ethereal)
                        {
                            if (Location.X > c[i].Location.X - 90f * c[i].Scale &&
                                Location.X < c[i].Location.X + 90f * c[i].Scale &&
                                Location.Y > c[i].Location.Y - 120f * c[i].Scale &&
                                Location.Y < c[i].Location.Y + 10f * c[i].Scale)
                            {
                                var dif = Math.Abs(Location.X - c[i].Location.X);
                                dif = 180f * c[i].Scale - dif;
                                dif *= 2f;
                                if (Location.X < c[i].Location.X)
                                {
                                    ColMove = -dif;
                                    c[i].ColMove = dif;
                                }
                                else
                                {
                                    ColMove = dif;
                                    c[i].ColMove = -dif;
                                }
                            }
                        }
                    }
                }
            }

            if (ColMove > 0f)
            {
                ColMove -= 400f * Game1.FrameTime;
                if (ColMove < 0f) ColMove = 0f;
            }
            else if (ColMove < 0f)
            {
                ColMove += 400f * Game1.FrameTime;
                if (ColMove > 0f) ColMove = 0f;
            }

            #endregion

            #region Update location by trajectory

            var pLoc = Location;

            if (State == CharState.Grounded)
            {
                if (Trajectory.X > 0)
                {
                    Trajectory.X -= Game1.Friction * Game1.FrameTime;
                    if (Trajectory.X < 0)
                        Trajectory.X = 0;
                }

                if (Trajectory.X < 0)
                {
                    Trajectory.X += Game1.Friction * Game1.FrameTime;
                    if (Trajectory.X > 0)
                        Trajectory.X = 0;
                }
            }

            Location.X += Trajectory.X * Game1.FrameTime;
            Location.X += ColMove * Game1.FrameTime;

            if (State == CharState.Air)
            {
                Location.Y += Trajectory.Y * Game1.FrameTime;
                Trajectory.Y += Game1.FrameTime * Game1.Gravity;
            }

            #endregion

            #region Colision detection

            if (State == CharState.Air)
            {
                CheckXCol(pLoc);

                #region Land on ledge

                if (Trajectory.Y > 0)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (Map.Ledges[i].TotalNodes > 1)
                        {
                            var ts = Map.GetLedgeSec(i, pLoc.X);
                            var s = Map.GetLedgeSec(i, Location.X);

                            if (s > -1 && ts > -1)
                            {
                                var tfy = Map.GetLedgeYLoc(i, s, pLoc.X);
                                var fy = Map.GetLedgeYLoc(i, s, Location.X);

                                if (pLoc.Y <= tfy && Location.Y >= fy)
                                {
                                    if (Trajectory.Y > 0)
                                    {
                                        Location.Y = fy;
                                        _ledgeAttach = i;
                                        Land();
                                    }
                                }
                                else
                                {
                                    if (Map.Ledges[i].Flags == (int)LedgeFlags.Solid && Location.Y >= fy)
                                    {
                                        Location.Y = fy;
                                        _ledgeAttach = i;
                                        Land();
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Land on col

                if (State == CharState.Air && Trajectory.Y > 0)
                {
                    if (Map.CheckCol(new Vector2(Location.X, Location.Y + 15)))
                    {
                        Location.Y = (int)(Location.Y + 15) / 64 * 64;
                        Land();
                    }
                }

                #endregion
            }
            else if (State == CharState.Grounded)
            {
                #region Grounded state

                if (_ledgeAttach > -1)
                {
                    if (Map.GetLedgeSec(_ledgeAttach, Location.X) == -1)
                    {
                        FallOff();
                    }
                    else
                    {
                        Location.Y = Map.GetLedgeYLoc(_ledgeAttach, Map.GetLedgeSec(_ledgeAttach, Location.X),
                            Location.X);
                    }
                }
                else
                {
                    if (!Map.CheckCol(new Vector2(Location.X, Location.Y + 15)))
                        FallOff();
                }

                CheckXCol(pLoc);

                #endregion
            }

            #endregion

            #region UpdateKey input

            if (AnimName == "idle" || AnimName == "run")
            {
                if (KeyLeft)
                {
                    SetAnim("run");
                    Trajectory.X = -200;
                    Face = CharDir.Left;
                }
                else if (KeyRight)
                {
                    SetAnim("run");
                    Trajectory.X = 200;
                    Face = CharDir.Right;
                }
                else
                {
                    SetAnim("idle");
                }

                if (KeyAttack)
                {
                    SetAnim("attack");
                }

                if (KeySecondary)
                {
                    SetAnim("second");
                }

                if (KeyJump)
                {
                    SetAnim("fly");
                    Trajectory.Y = -600;
                    State = CharState.Air;
                    _ledgeAttach = -1;
                    if (KeyRight) Trajectory.X = 200;
                    else if (KeyLeft) Trajectory.X = -200;
                }
            }

            if (AnimName == "fly")
            {
                if (KeyLeft)
                {
                    Face = CharDir.Left;
                    if (Trajectory.X > -200)
                        Trajectory.X -= 500 * Game1.FrameTime;
                }
                else if (KeyRight)
                {
                    Face = CharDir.Right;
                    if (Trajectory.X < 200)
                        Trajectory.X += 500 * Game1.FrameTime;
                }
            }

            PressedKey = PressedKeys.None;

            if (KeyAttack)
                PressedKey = KeyUp ? PressedKeys.Lower : KeyDown ? PressedKeys.Upper : PressedKeys.Attack;

            if (KeySecondary)
                PressedKey = KeyUp ? PressedKeys.SecUp : KeyDown ? PressedKeys.SecDown : PressedKeys.Secondary;

            if (PressedKey != PressedKeys.None)
            {
                if (GotoGoal[(int)PressedKey] > -1)
                {
                    AnimFrame = GotoGoal[(int)PressedKey];

                    if (KeyLeft)
                        Face = CharDir.Left;
                    else if (KeyRight)
                        Face = CharDir.Right;

                    PressedKey = PressedKeys.None;

                    for (var i = 0; i < GotoGoal.Length; i++)
                        GotoGoal[i] = -1;

                    _frame = 0;
                    _script.DoScript(Anim, AnimFrame);
                }
            }

            #endregion
        }

        private void CheckTrig(ParticleManager pMan)
        {
            var frameIndex = _charDef.Animations[Anim].KeyFrames[AnimFrame].FrameRef;

            var frame = _charDef.Frames[frameIndex];

            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index >= 1000)
                {
                    var location = part.Location * Scale + Location;

                    if (Face == CharDir.Left)
                        location.X -= part.Location.X * Scale * 2;

                    FireTrig(part.Index - 1000, location, pMan);
                }
            }
        }

        private void FireTrig(int trig, Vector2 loc, ParticleManager pMan)
        {
            switch (trig)
            {
                case TrigPistolAcross:
                    pMan.MakeBullet(loc, new Vector2(2000, 0), Face, Id);
                    Sound.PlayCue("revol");
                    break;
                case TrigPistolDown:
                    pMan.MakeBullet(loc, new Vector2(1400, 1400), Face, Id);
                    Sound.PlayCue("revol");
                    break;
                case TrigPistolUp:
                    pMan.MakeBullet(loc, new Vector2(1400, -1400), Face, Id);
                    Sound.PlayCue("revol");
                    break;
                case TrigBloodSquirtBack:
                case TrigBloodSquirtDown:
                case TrigBloodSquirtDownBack:
                case TrigBloodSquirtDownForward:
                case TrigBloodSquirtForward:
                case TrigBloodSquirtUp:
                case TrigBloodSquirtUpBack:
                case TrigBloodSquirtUpForward:
                    var r = 0.0;
                    switch (trig)
                    {
                        case TrigBloodSquirtForward:
                            r = 0.0;
                            break;
                        case TrigBloodSquirtDownForward:
                            r = Math.PI * .25;
                            break;
                        case TrigBloodSquirtDown:
                            r = Math.PI * .5;
                            break;
                        case TrigBloodSquirtDownBack:
                            r = Math.PI * .75;
                            break;
                        case TrigBloodSquirtBack:
                            r = Math.PI;
                            break;
                        case TrigBloodSquirtUpBack:
                            r = Math.PI * 1.25;
                            break;
                        case TrigBloodSquirtUp:
                            r = Math.PI * 1.5;
                            break;
                        case TrigBloodSquirtUpForward:
                            r = Math.PI * 1.75;
                            break;
                    }

                    for (var i = 0; i < 7; i++)
                    {
                        pMan.AddParticle(new Blood(loc,
                            new Vector2((float)Math.Cos(r) * (Face == CharDir.Right ? 1f : -1f), (float)Math.Sin(r)) *
                            Rand.GetRandomFloat(10f, 500f) + Rand.GetRandomVector2(-90f, 90f, -90f, 90f), 1f, 0f, 0f, 1f,
                            Rand.GetRandomFloat(0.1f, 0.5f), Rand.GetRandomInt(0, 4)));
                    }
                    pMan.AddParticle(new BloodDust(loc, Rand.GetRandomVector2(-30f, 30f, -30f, 30f), 1f, 0f, 0f, .2f,
                        Rand.GetRandomFloat(.25f, .5f), Rand.GetRandomInt(0, 4)));
                    break;
                case TrigBloodCloud:
                    pMan.AddParticle(new BloodDust(loc, Rand.GetRandomVector2(-30f, 30f, -30f, 30f), 1f, 0f, 0f, .4f,
                        Rand.GetRandomFloat(.25f, .75f), Rand.GetRandomInt(0, 4)));
                    break;
                case TrigBloodSplat:
                    for (var i = 0; i < 6; i++)
                    {
                        pMan.AddParticle(new BloodDust(loc, Rand.GetRandomVector2(-30f, 30f, -30f, 30f), 1f, 0f, 0f, .4f,
                            Rand.GetRandomFloat(.025f, .125f), Rand.GetRandomInt(0, 4)));
                    }
                    break;

                default:
                    pMan.AddParticle(new Hit(loc, new Vector2(200f * (float)Face - 100f, 0f), Id, trig));
                    break;
            }

            switch (trig)
            {
                case TrigPistolAcross:
                case TrigPistolUp:
                case TrigPistolDown:
                    if (Team == TeamGoodGuys && Id < 4)
                    {
                        QuakeManager.SetRumble(Id, 1, .5f);
                        QuakeManager.SetRumble(Id, 0, .3f);
                    }
                    break;
            }
        }

        private void CheckXCol(Vector2 pLoc)
        {
            if (Trajectory.X + ColMove > 0)
            {
                if (Map.CheckCol(new Vector2(Location.X + 25, Location.Y - 15)))
                    Location.X = pLoc.X;
            }
            else if (Trajectory.X + ColMove < 0)
            {
                if (Map.CheckCol(new Vector2(Location.X - 25, Location.Y - 15)))
                    Location.X = pLoc.X;
            }
        }

        private void FallOff()
        {
            State = CharState.Air;
            SetAnim("fly");
            Trajectory.Y = 0;
        }

        private void Land()
        {
            State = CharState.Grounded;
            switch (AnimName)
            {
                case "jhit":
                case "jmid":
                case "jfall":
                    SetAnim("hitland");
                    if (Hp < 0)
                        SetAnim("dieland");
                    break;
                default:
                    SetAnim("land");
                    break;
            }
        }

        public void Slide(float distance)
        {
            Trajectory.X = (float)Face * 2f * distance - distance;
        }

        public void SetJump(float jump)
        {
            Trajectory.Y = -jump;
            State = CharState.Air;
            _ledgeAttach = -1;
        }

        public bool InHitBounds(Vector2 hitLoc)
        {
            return hitLoc.X > Location.X - 50f * Scale &&
                   hitLoc.X < Location.X + 50f * Scale &&
                   hitLoc.Y > Location.Y - 190f * Scale &&
                   hitLoc.Y < Location.Y + 10f * Scale;
        }

        public void DoInput()
        {
            KeyLeft = ControlInput.KeyLeft;
            KeyRight = ControlInput.KeyRight;
            KeyUp = ControlInput.KeyUp;
            KeyDown = ControlInput.KeyDown;
            KeyAttack = ControlInput.KeyAttack;
            KeyJump = ControlInput.KeyJump;
            KeySecondary = ControlInput.KeySecondary;
        }

        private void InitScript()
        {
            SetAnim("init");
            if (AnimName == "init")
            {
                for (var i = 0; i < _charDef.Animations[Anim].KeyFrames.Length; i++)
                {
                    if (_charDef.Animations[Anim].KeyFrames[i].FrameRef > -1)
                        _script.DoScript(Anim, i);
                }
            }
        }

        public void KillMe()
        {
            if (DyingFrame < 0f)
                DyingFrame = 0f;
        }

        public void SetFrame(int newFrame)
        {
            AnimFrame = newFrame;
            _frame = 0f;
            for (int i = 0; i < GotoGoal.Length; i++)
                GotoGoal[i] = -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sRect = new Rectangle();

            var frameIndex = _charDef.Animations[Anim].KeyFrames[AnimFrame].FrameRef;
            var frame = _charDef.Frames[frameIndex];

            spriteBatch.Begin();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index > -1 && part.Index < 1000)
                {
                    sRect.X = ((part.Index % 64) % 5) * 64;
                    sRect.Y = ((part.Index % 64) / 5) * 64;
                    sRect.Width = 64;
                    sRect.Height = 64;

                    if (part.Index >= 192)
                    {
                        sRect.X = ((part.Index % 64) % 3) * 80;
                        sRect.Width = 80;
                    }

                    var rotation = part.Rotation;

                    var location = part.Location * Scale + Location - Game1.Scroll;
                    var scaling = part.Scaling * Scale;
                    if (part.Index >= 128) scaling *= 1.35f;

                    if (Face == CharDir.Left)
                    {
                        rotation = -rotation;
                        location.X -= part.Location.X * Scale * 2.0f;
                    }

                    Texture2D texture = null;

                    var t = part.Index / 64;
                    switch (t)
                    {
                        case 0:
                            texture = HeadTex[_charDef.HeadIndex];
                            break;
                        case 1:
                            texture = TorsoTex[_charDef.TorsoIndex];
                            break;
                        case 2:
                            texture = LegsTex[_charDef.LegsIndex];
                            break;
                        case 3:
                            texture = WeaponTex[_charDef.WeaponIndex];
                            break;
                    }

                    var color = new Color(new Vector4(1));

                    if (DyingFrame > 0f)
                        color = new Color(new Vector4(1 - DyingFrame));


                    var flip = (Face == CharDir.Right && part.Flip == 0) || (Face == CharDir.Left && part.Flip == 1);

                    if (texture != null)
                    {
                        spriteBatch.Draw(
                            texture,
                            location,
                            sRect,
                            color,
                            rotation,
                            new Vector2(sRect.Width / 2f, 32f),
                            scaling,
                            (flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
                            1.0f
                            );
                    }
                }
            }

            spriteBatch.End();
        }
    }
}