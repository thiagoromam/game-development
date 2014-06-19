using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.Input;

namespace ZombieSmashers.Gui
{
    public class Menu
    {
        public float TransFrame = 1f;
        public Trans TransType = Trans.All;
        public Level TransGoal;
        public Level Level = Level.Main;
        public int SelItem;
        private readonly int[] _levelSel = new int[32];
        private readonly Option[] _option = new Option[10];
        private readonly float[] _optionFrame = new float[10];
        private int _totalOptions;
        public MenuMode MenuMode = MenuMode.Main;

        private readonly Texture2D _poseTex;
        private readonly Texture2D _poseForeTex;
        private readonly Texture2D _optionsTex;
        private readonly Texture2D _backTex;
        private readonly Texture2D _spritesTex;
        private readonly SpriteBatch _sprite;

        private readonly Vector2[] _fog = new Vector2[128];
        private float _frame;
        
        public Menu(Texture2D poseTex, Texture2D poseForeTex, Texture2D optionsTex, Texture2D backTex,
            Texture2D spritesTex, SpriteBatch sprite)
        {
            _poseTex = poseTex;
            _poseForeTex = poseForeTex;
            _optionsTex = optionsTex;
            _sprite = sprite;
            _backTex = backTex;
            _spritesTex = spritesTex;

            for (var i = 0; i < _fog.Length; i++)
            {
                _fog[i] = Rand.GetRandomVector2(-50f, Game1.ScreenSize.X + 50f, Game1.ScreenSize.Y - 100f,
                    Game1.ScreenSize.Y);
            }
        }

        public void Update(Game1 game)
        {
            _frame += Game1.FrameTime / 2f;
            if (_frame > 6.28f)
                _frame -= 6.28f;

            if (TransFrame < 2f)
            {
                var pFrame = TransFrame;
                TransFrame += Game1.FrameTime;

                if (TransType == Trans.Buttons)
                    TransFrame += Game1.FrameTime;

                if (pFrame < 1f && TransFrame >= 1f)
                {
                    _levelSel[(int)Level] = SelItem;
                    Level = TransGoal;
                    SelItem = _levelSel[(int)Level];

                    switch (Level)
                    {
                        case Level.NewGame:
                            game.NewGame();
                            break;
                        case Level.ResumeGame:
                            Game1.GameMode = GameModes.Playing;
                            break;
                        case Level.EndGame:
                            MenuMode = MenuMode.Main;
                            Level = Level.Main;
                            break;
                        case Level.Quit:
                            game.Quit();
                            break;
                    }
                }
            }

            for (var i = 0; i < _fog.Length; i++)
            {
                _fog[i].X -= Game1.FrameTime * (50f + (i % 20 + 2));
                _fog[i].Y += Game1.FrameTime * (i % 14 + 5);

                if (_fog[i].X < -150f)
                {
                    _fog[i].X = Game1.ScreenSize.X + Rand.GetRandomFloat(150f, 200f);
                    _fog[i].Y = Game1.ScreenSize.Y - Rand.GetRandomFloat(0f, 300f);
                }
            }

            for (var i = 0; i < _optionFrame.Length; i++)
            {
                if (SelItem == i)
                {
                    if (_optionFrame[i] < 1f)
                    {
                        _optionFrame[i] += Game1.FrameTime * 7f;
                        if (_optionFrame[i] > 1f)
                            _optionFrame[i] = 1f;
                    }
                }
                else
                {
                    if (_optionFrame[i] > 0f)
                    {
                        _optionFrame[i] -= Game1.FrameTime * 4f;
                        if (_optionFrame[i] < 0f)
                            _optionFrame[i] = 0f;
                    }
                }
            }

            PopulateOptions();

            if (_totalOptions > 0)
            {
                if (ControlInput.KeyUpPressed)
                {
                    SelItem = (SelItem + (_totalOptions - 1)) % _totalOptions;
                }
                else if (ControlInput.KeyDownPressed)
                {
                    SelItem = (SelItem + 1) % _totalOptions;
                }
            }

            var ok = false;
            if (TransFrame > 1.9f)
            {
                if (ControlInput.KeyAttackPressed)
                    ok = true;

                if (ControlInput.KeyStartPressed)
                {
                    if (MenuMode == MenuMode.Main || MenuMode == MenuMode.Dead)
                        ok = true;
                    else
                        Transition(Level.ResumeGame, true);
                }

                if (ok)
                {
                    switch (Level)
                    {
                        case Level.Main:
                            switch (_option[SelItem])
                            {
                                case Option.NewGame:
                                    Transition(Level.NewGame, true);
                                    break;
                                case Option.ResumeGame:
                                    Transition(Level.ResumeGame, true);
                                    break;
                                case Option.EndGame:
                                    Transition(Level.EndGame, true);
                                    break;
                                case Option.Continue:
                                    break;
                                case Option.Options:
                                    Transition(Level.Options);
                                    break;
                                case Option.Quit:
                                    Transition(Level.Quit, true);
                                    break;
                            }
                            break;

                        case Level.Dead:
                            switch (_option[SelItem])
                            {
                                case Option.EndGame:
                                    Transition(Level.EndGame, true);
                                    break;
                                case Option.Quit:
                                    Transition(Level.Quit, true);
                                    break;
                            }
                            break;

                        case Level.Options:
                            switch (_option[SelItem])
                            {
                                case Option.Back:
                                    Transition(Level.Main);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void Transition(Level goal, bool all = false)
        {
            TransGoal = goal;
            TransFrame = 0f;
            TransType = all ? Trans.All : Trans.Buttons;
        }

        private float GetAlpha(bool buttons)
        {
            if (!buttons && TransType == Trans.Buttons)
                return 1f;

            if (TransFrame < 2f)
            {
                if (TransFrame < 1f) return 1f - TransFrame;
                return TransFrame - 1f;
            }

            return 1f;
        }

        public void Draw()
        {
            _sprite.Begin();

            if (MenuMode == MenuMode.Main)
            {
                _sprite.Draw(_backTex, new Rectangle(0, 0, 1280, 720),
                    new Color(new Vector4(GetAlpha(false), GetAlpha(false), GetAlpha(false), 1f)));
            }
            else if (MenuMode == MenuMode.Pause)
            {
                _sprite.Draw(_backTex, new Rectangle(0, 0, 1280, 720), new Rectangle(600, 400, 200, 200),
                    new Color(new Vector4(1f, 1f, 1f, .5f)));
            }

            _sprite.End();

            _sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            var pan = (float)Math.Cos(_frame) * 10f + 10f;
            for (var i = _fog.Length / 2; i < _fog.Length; i++)
            {
                _sprite.Draw(_spritesTex, _fog[i] + new Vector2(pan, 0f), new Rectangle((i % 4) * 64, 0, 64, 64),
                    new Color(new Vector4(1f, 1f, 1f, .1f * GetAlpha(false))), (_fog[i].X + _fog[i].Y) / 100f,
                    new Vector2(32f, 32f), i % 10 * .5f + 2f, SpriteEffects.None, 1f);
            }

            _sprite.End();

            _sprite.Begin();

            var poseA = GetAlpha(false);
            if (MenuMode != MenuMode.Dead)
            {
                if (MenuMode != MenuMode.Main)
                    poseA = 0f;

                _sprite.Draw(_poseTex,
                    new Vector2(Game1.ScreenSize.X - (Game1.ScreenSize.Y / 480f) * 380f * (.5f * GetAlpha(false) + .5f) +
                                (float)Math.Cos(_frame) * 10f + 10f, 0f), new Rectangle(0, 0, 421, 480),
                    new Color(new Vector4(poseA, poseA, poseA, 1f)), 0f, new Vector2(), (Game1.ScreenSize.Y / 480f),
                    SpriteEffects.None, 1f);
            }

            PopulateOptions();

            for (var i = 0; i < _totalOptions; i++)
            {
                _sprite.Draw(_optionsTex,
                    new Vector2(190f + i * 5f + pan + _optionFrame[i] * 10f + GetAlpha(true) * 50f,
                        300f + i * 64f - _totalOptions * 32f), new Rectangle(0, (int)_option[i] * 64, 320, 64),
                    new Color(new Vector4(1f, 1f - _optionFrame[i], 1f - _optionFrame[i], GetAlpha(true))),
                    (1f - _optionFrame[i]) * -.1f, new Vector2(160f, 32f), 1f, SpriteEffects.None, 1f);
            }

            _sprite.End();

            if (MenuMode != MenuMode.Dead)
            {
                _sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                pan *= 2f;
                for (var i = 0; i < _fog.Length / 2; i++)
                {
                    _sprite.Draw(_spritesTex, _fog[i] + new Vector2(pan, 0f), new Rectangle((i % 4) * 64, 0, 64, 64),
                        new Color(new Vector4(1f, 1f, 1f, .1f * GetAlpha(false))), (_fog[i].X + _fog[i].Y) / 100f,
                        new Vector2(32f, 32f), i % 10 * .5f + 2f, SpriteEffects.None, 1f);
                }

                _sprite.End();
                _sprite.Begin();

                _sprite.Draw(_poseForeTex,
                    new Vector2(
                        Game1.ScreenSize.X - (Game1.ScreenSize.Y / 480f) * 616f * GetAlpha(false) +
                        (float)Math.Cos(_frame) * 20f + 20f, Game1.ScreenSize.Y - (Game1.ScreenSize.Y / 480f) * 286f),
                    new Rectangle(0, 0, 616, 286),
                    new Color(new Vector4(GetAlpha(false), GetAlpha(false), GetAlpha(false), 1f)), 0f, new Vector2(),
                    (Game1.ScreenSize.Y / 480f), SpriteEffects.None, 1f);

                _sprite.End();
            }
        }

        private void PopulateOptions()
        {
            for (var i = 0; i < _option.Length; i++)
                _option[i] = Option.None;

            switch (Level)
            {
                case Level.Main:
                    if (MenuMode == MenuMode.Pause)
                    {
                        _option[0] = Option.ResumeGame;
                        _option[1] = Option.EndGame;
                        _option[2] = Option.Options;
                        _option[3] = Option.Quit;
                        _totalOptions = 4;
                    }
                    else
                    {
                        _option[0] = Option.NewGame;
                        _option[1] = Option.Continue;
                        _option[2] = Option.Options;
                        _option[3] = Option.Quit;
                        _totalOptions = 4;
                    }
                    break;
                case Level.Options:
                    _option[0] = Option.Back;
                    _totalOptions = 1;
                    break;
                case Level.Dead:
                    _option[0] = Option.EndGame;
                    _option[1] = Option.Quit;
                    _totalOptions = 2;
                    break;
                default:
                    _totalOptions = 0;
                    break;
            } 
        }

        public void Pause()
        {
            MenuMode = MenuMode.Pause;
            Game1.GameMode = GameModes.Menu;
            TransFrame = 1f;
            Level = Level.Main;
            TransType = Trans.All;
        }

        public void Die()
        {
            MenuMode = MenuMode.Dead;
            Game1.GameMode = GameModes.Menu;
            TransFrame = 1f;
            Level = Level.Dead;
            TransType = Trans.All;
        }
    }
}