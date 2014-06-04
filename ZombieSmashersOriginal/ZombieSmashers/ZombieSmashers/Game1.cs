using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Input;
using ZombieSmashers.MapClasses;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public const float Friction = 1000;
        public const float Gravity = 900;
        public static Vector2 Scroll;
        public static float FrameTime;

        private readonly Vector2 _scrollOffset = new Vector2(400);
        private readonly Texture2D[] _mapsTex = new Texture2D[1];
        private readonly Character[] _characters = new Character[16];
        private readonly CharDef[] _charDefs = new CharDef[16];
        private Map _map;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _map = new Map("maps/map");

            _charDefs[(int)CharacterType.Guy] = new CharDef("chars/guy");
            _characters[0] = new Character(new Vector2(100, 100), _charDefs[(int)CharacterType.Guy]) { Map = _map };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (var i = 0; i < _mapsTex.Length; i++)
                _mapsTex[i] = Content.Load<Texture2D>("gfx/maps" + (i + 1));

            Character.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            FrameTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (_characters[0] != null)
            {
                Scroll += (_characters[0].Location - _scrollOffset - Scroll) * FrameTime * 20;

                ControlInput.Update();
                _characters[0].DoInput();
            }

            for (var i = 0; i < _characters.Length; i++)
            {
                var character = _characters[i];
                if (character != null)
                    character.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _map.Draw(_spriteBatch, _mapsTex, 0, 2);

            _characters[0].Draw(_spriteBatch);

            _map.Draw(_spriteBatch, _mapsTex, 2, 3);

            base.Draw(gameTime);
        }
    }
}