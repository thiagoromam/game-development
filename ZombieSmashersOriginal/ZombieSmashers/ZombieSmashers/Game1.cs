using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.Audio;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Gui;
using ZombieSmashers.Input;
using ZombieSmashers.MapClasses;
using ZombieSmashers.Particles;
using ZombieSmashers.Shakes;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _mainTarget;

        public const float Friction = 1000;
        public const float Gravity = 900;

        private readonly Vector2 _scrollOffset = new Vector2(400);
        private readonly Texture2D[] _mapsTex = new Texture2D[1];
        private readonly Texture2D[] _mapBackTex = new Texture2D[1];
        private readonly Character[] _characters = new Character[16];
        public static readonly CharDef[] CharDefs = new CharDef[16];
        private Map _map;
        private static Vector2 _scroll;
        private static Vector2 _screenSize;
        private ParticleManager _particleManager;
        private Texture2D _spritesTex;
        private Texture2D _nullTex;

        private Hud _hud;
        public static Menu Menu;
        public static long Score;
        public static GameModes GameMode;
        public static float SlowTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
        }

        public static Vector2 Scroll
        {
            get { return _scroll; }
        }
        public static float FrameTime { get; private set; }
        public static Vector2 ScreenSize
        {
            get { return _screenSize; }
        }

        protected override void Initialize()
        {
            _map = new Map("start");

            CharDefs[(int)CharacterType.Guy] = new CharDef("chars/guy");
            CharDefs[(int)CharacterType.Zombie] = new CharDef("chars/zombie");

            _characters[0] = new Character(new Vector2(100, 100), CharDefs[(int)CharacterType.Guy], 0,
                Character.TeamGoodGuys) { Map = _map };

            _screenSize.X = GraphicsDevice.Viewport.Width;
            _screenSize.Y = GraphicsDevice.Viewport.Height;

            base.Initialize();

            _particleManager = new ParticleManager(_spriteBatch);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (var i = 0; i < _mapsTex.Length; i++)
                _mapsTex[i] = Content.Load<Texture2D>("gfx/maps" + (i + 1));

            for (var i = 0; i < _mapBackTex.Length; i++)
                _mapBackTex[i] = Content.Load<Texture2D>("gfx/back" + (i + 1));

            _spritesTex = Content.Load<Texture2D>("gfx/sprites");

            Character.LoadContent(Content);

            _mainTarget = new RenderTarget2D(
                GraphicsDevice,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24
            );

            _nullTex = Content.Load<Texture2D>(@"gfx/1x1");

            Menu = new Menu(Content.Load<Texture2D>(@"gfx/pose"), Content.Load<Texture2D>(@"gfx/posefore"),
                Content.Load<Texture2D>(@"gfx/options"), _mapBackTex[0], _spritesTex, _spriteBatch);

            _hud = new Hud(_spriteBatch, _spritesTex, _nullTex, _characters, _map);
        }

        protected override void Update(GameTime gameTime)
        {
            Sound.Update();
            Music.Play("music1");
            QuakeManager.Update();
            ControlInput.Update();

            FrameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (SlowTime > 0f)
            {
                SlowTime -= FrameTime;
                FrameTime /= 10f;
            }

            switch (GameMode)
            {
                case GameModes.Playing:
                    UpdateGame();
                    break;
                case GameModes.Menu:
                    if (Menu.MenuMode == MenuMode.Dead)
                    {
                        var pTime = FrameTime;
                        FrameTime /= 3f;
                        UpdateGame();
                        FrameTime = pTime;
                    }
                    Menu.Update(this);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateGame()
        {
            if (_map.TransOutFrame <= 0f)
            {
                _particleManager.UpdateParticles(FrameTime, _map, _characters);

                if (_characters[0] != null)
                {
                    _scroll += (_characters[0].Location - _scrollOffset - _scroll) * FrameTime * 20;
                    _scroll += QuakeManager.Quake.Vector;

                    var xLim = _map.GetXLim();
                    var yLim = _map.GetYLim();

                    if (_scroll.X < 0) _scroll.X = 0;
                    if (_scroll.X > xLim) _scroll.X = xLim;
                    if (_scroll.Y < 0) _scroll.Y = 0;
                    if (_scroll.Y > yLim) _scroll.Y = yLim;

                    _characters[0].DoInput();
                }

                for (var i = 0; i < _characters.Length; i++)
                {
                    var character = _characters[i];
                    if (character != null)
                    {
                        character.Update(_particleManager, _characters);
                        if (character.DyingFrame > 1f)
                            _characters[i] = null;
                    }
                }
            }

            _map.Update(_particleManager, _characters);
            _hud.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (GameMode)
            {
                case GameModes.Playing:
                    DrawGame();
                    _hud.Draw();
                    break;
                case GameModes.Menu:
                    if (Menu.MenuMode == MenuMode.Pause || Menu.MenuMode == MenuMode.Dead)
                        DrawGame();
                    
                    Menu.Draw();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            GraphicsDevice.SetRenderTarget(_mainTarget);
            GraphicsDevice.Clear(Color.Black);

            _map.Draw(_spriteBatch, _mapsTex, _mapBackTex, 0, 2);
            _particleManager.DrawParticles(_spritesTex, true);
            for (var i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != null)
                    _characters[i].Draw(_spriteBatch);
            }

            _particleManager.DrawParticles(_spritesTex, false);
            _map.Draw(_spriteBatch, _mapsTex, _mapBackTex, 2, 3);

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

            _spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.White);

            _spriteBatch.End();

            if (QuakeManager.Blast.Value > 0)
            {
                _spriteBatch.Begin();

                for (var i = 0; i < 5; i++)
                {
                    _spriteBatch.Draw(
                        _mainTarget,
                        QuakeManager.Blast.Center - Scroll,
                        new Rectangle(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y),
                        new Color(new Vector4(1f, 1f, 1f, 0.35f * (QuakeManager.Blast.Value / QuakeManager.Blast.Magnitude))),
                        0f,
                        QuakeManager.Blast.Center - Scroll,
                        (1 + (QuakeManager.Blast.Magnitude - QuakeManager.Blast.Value) * 0.1f + ((i + 1) / 40f)),
                        SpriteEffects.None,
                        1f
                    );
                }

                _spriteBatch.End();
            }
        }

        public void NewGame()
        {
            GameMode = GameModes.Playing;

            _characters[0] = new Character(new Vector2(100f, 100f), CharDefs[(int)CharacterType.Guy], 0,
                Character.TeamGoodGuys) { Map = _map };
            _characters[0].Hp = _characters[0].Mhp = 100;
            for (var i = 1; i < _characters.Length; i++) _characters[i] = null;

            _particleManager.Reset();
            _map.Path = "start";
            _map.GlobalFlags = new MapFlags(64);
            _map.Read();
            _map.TransDir = TransitionDirection.Intro;
            _map.TransInFrame = 1f;
        }

        public void Quit()
        {
            Exit();
        }
    }
}