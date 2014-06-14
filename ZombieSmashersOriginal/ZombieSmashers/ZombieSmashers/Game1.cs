using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieSmashers.Audio;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Input;
using ZombieSmashers.MapClasses;
using ZombieSmashers.Particles;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers
{
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public const float Friction = 1000;
        public const float Gravity = 900;

        private readonly Vector2 _scrollOffset = new Vector2(400);
        private readonly Texture2D[] _mapsTex = new Texture2D[1];
        private readonly Texture2D[] _mapBackTex = new Texture2D[1];
        private readonly Character[] _characters = new Character[16];
        private readonly CharDef[] _charDefs = new CharDef[16];
        private Map _map;
        private static Vector2 _scroll;
        private static Vector2 _screenSize;
        private ParticleManager _particleManager;
        private Texture2D _spritesTex;

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

        public static float SlowTime { get; set; }

        public static float FrameTime { get; private set; }

        public static Vector2 ScreenSize
        {
            get { return _screenSize; }
        }

        protected override void Initialize()
        {
            _map = new Map("maps/map");

            _charDefs[(int) CharacterType.Guy] = new CharDef("chars/guy");
            _charDefs[(int) CharacterType.Zombie] = new CharDef("chars/zombie");

            _characters[0] = new Character(new Vector2(100, 100), _charDefs[(int) CharacterType.Guy], 0,
                Character.TeamGoodGuys) {Map = _map};

            for (var i = 1; i < 9; i++)
            {
                _characters[i] = new Character(new Vector2(i*100, 100), _charDefs[(int) CharacterType.Zombie], i,
                    Character.TeamBadGuys) {Map = _map};
            }

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            FrameTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (SlowTime > 0f)
            {
                SlowTime -= FrameTime;
                FrameTime /= 10f;
            }

            _particleManager.UpdateParticles(FrameTime, _map, _characters);

            if (_characters[0] != null)
            {
                _scroll += (_characters[0].Location - _scrollOffset - _scroll)*FrameTime*20;

                var xLim = _map.GetXLim();
                var yLim = _map.GetYLim();

                if (_scroll.X < 0) _scroll.X = 0;
                if (_scroll.X > xLim) _scroll.X = xLim;
                if (_scroll.Y < 0) _scroll.Y = 0;
                if (_scroll.Y > yLim) _scroll.Y = yLim;

                ControlInput.Update();
                _characters[0].DoInput();
            }

            for (var i = 0; i < _characters.Length; i++)
            {
                var character = _characters[i];
                if (character != null)
                    character.Update(gameTime, _particleManager, _characters);
            }

            _map.Update(_particleManager);

            Sound.Update();
            Music.Play("music1");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _map.Draw(_spriteBatch, _mapsTex, _mapBackTex, 0, 2);
            _particleManager.DrawParticles(_spritesTex, true);
            for (var i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != null)
                    _characters[i].Draw(_spriteBatch);
            }

            _particleManager.DrawParticles(_spritesTex, false);
            _map.Draw(_spriteBatch, _mapsTex, _mapBackTex, 2, 3);

            base.Draw(gameTime);
        }
    }
}