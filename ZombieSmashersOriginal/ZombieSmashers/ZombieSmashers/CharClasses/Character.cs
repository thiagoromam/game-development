using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieSmashers.Input;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.CharClasses
{
    public class Character
    {
        public static Texture2D[] HeadTex = new Texture2D[1];
        public static Texture2D[] TorsoTex = new Texture2D[1];
        public static Texture2D[] LegsTex = new Texture2D[1];
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

        // Control fields
        private bool _keyLeft;
        private bool _keyRight;
        private bool _keyUp;
        private bool _keyDown;
        private bool _keyJump;
        private bool _keyAttack;
        private bool _keySecondary;
        private readonly CharDef _charDef;
        private int _ledgeAttach = -1;

        public Map Map;

        public Character(Vector2 newLoc, CharDef newCharDef)
        {
            Location = newLoc;
            Trajectory = Vector2.Zero;

            Face = CharDir.Left;
            Scale = 0.5f;
            _charDef = newCharDef;

            SetAnim("fly");

            State = CharState.Air;
        }

        private void SetAnim(string newAnim)
        {
            if (newAnim == AnimName)
                return;

            for (var i = 0; i < _charDef.Animations.Length; i++)
            {
                if (_charDef.Animations[i].Name == newAnim)
                {
                    Anim = i;
                    AnimFrame = 0;
                    _frame = 0;
                    AnimName = newAnim;

                    break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            #region Update animation

            var animation = _charDef.Animations[Anim];
            var keyFrame = animation.KeyFrames[AnimFrame];

            _frame += Game1.FrameTime * 30;

            if (_frame > keyFrame.Duration)
            {
                _frame -= keyFrame.Duration;
                AnimFrame++;

                if (AnimFrame >= animation.KeyFrames.Length)
                    AnimFrame = 0;

                keyFrame = animation.KeyFrames[AnimFrame];

                if (keyFrame.FrameRef <= 0)
                    AnimFrame = 0;
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
                if (_keyLeft)
                {
                    SetAnim("run");
                    Trajectory.X = -200;
                    Face = CharDir.Left;
                }
                else if (_keyRight)
                {
                    SetAnim("run");
                    Trajectory.X = 200;
                    Face = CharDir.Right;
                }
                else
                {
                    SetAnim("idle");
                }
                if (_keyJump)
                {
                    SetAnim("fly");
                    Trajectory.Y = -600;
                    State = CharState.Air;
                    _ledgeAttach = -1;
                    if (_keyRight) Trajectory.X = 200;
                    else if (_keyLeft) Trajectory.X = -200;
                }
            }

            if (AnimName == "fly")
            {
                if (_keyLeft)
                {
                    Face = CharDir.Left;
                    if (Trajectory.X > -200)
                        Trajectory.X -= 500 * Game1.FrameTime;
                }
                else if (_keyRight)
                {
                    Face = CharDir.Right;
                    if (Trajectory.X < 200)
                        Trajectory.X += 500 * Game1.FrameTime;
                }
            }

            #endregion
        }

        private void CheckXCol(Vector2 pLoc)
        {
            if (Trajectory.X > 0)
            {
                if (Map.CheckCol(new Vector2(Location.X + 25, Location.Y - 15)))
                    Location.X = pLoc.X;
            }
            else if (Trajectory.X < 0)
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
            SetAnim("idle");
        }

        public void DoInput()
        {
            _keyLeft = ControlInput.KeyLeft;
            _keyRight = ControlInput.KeyRight;
            _keyUp = ControlInput.KeyUp;
            _keyDown = ControlInput.KeyDown;
            _keyAttack = ControlInput.KeyAttack;
            _keyJump = ControlInput.KeyJump;
            _keySecondary = ControlInput.KeySecondary;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sRect = new Rectangle();

            var frameIndex = _charDef.Animations[Anim].KeyFrames[AnimFrame].FrameRef;
            var frame = _charDef.Frames[frameIndex];

            spriteBatch.Begin();

            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index > -1)
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

                    var color = new Color(new Vector4(1f, 1f, 1f, 1f));

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
                            SpriteEffects.None,
                            1.0f
                            );
                    }
                }
            }

            spriteBatch.End();
        }
    }
}