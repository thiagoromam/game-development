using CharacterEditor.Editor;
using CharacterEditor.Ioc;
using Funq.Fast;
using Helpers;
using KeyboardLib.Api;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MouseLib.Api;
using SharedLib;
using SharedLib.Mouse;
using TextLib.Api;

namespace CharacterEditor
{
    public class GameRoot : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IMouseComponent _mouseComponent;
        private IMouseDrawer _mouseDrawer;
        private IMouseInput _mouseInput;
        private IKeyboardComponent _keyboardComponent;
        private IText _text;
        private ITextContent _textContent;
        private CharacterBoard _characterBoard;
        private GuiManager _guiManager;

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            App.Register();

            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _mouseComponent = DependencyInjection.Resolve<IMouseComponent>();
            _mouseDrawer = DependencyInjection.Resolve<IMouseDrawer>();
            _characterBoard = new CharacterBoard();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);
            SharedArt.LoadContent(Content);

            App.Register(_spriteBatch);
            LibContent.SetContents();

            _text = DependencyInjection.Resolve<IText>();
            _textContent = DependencyInjection.Resolve<ITextContent>();
            _keyboardComponent = DependencyInjection.Resolve<IKeyboardComponent>();
            _textContent.Size = 0.75f;

            _guiManager = new GuiManager();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _mouseComponent.Update();
            _keyboardComponent.Update();
            _guiManager.Update();
            _characterBoard.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(300, 450, 200, 5), new Color(255, 0, 0, 0.5f));
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(0, 0, 200, 450), new Color(0, 0, 0, 0.5f));
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(590, 0, 300, 600), new Color(0, 0, 0, 0.5f));
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(200, 0, 150, 50), new Color(0, 0, 0, 0.5f));
            _spriteBatch.End();

            _guiManager.Draw(_spriteBatch);
            _characterBoard.Draw(_spriteBatch);
            _mouseDrawer.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        // Suport
        // ReSharper disable UnusedMember.Local
        public bool DrawClickText(int x, int y, string s)
        {
            var position = new Vector2(x, y);

            if (_text.MouseIntersects(s, position))
            {
                _text.Draw(s, position, Color.Yellow);
                return _mouseInput.LeftButtonClick;
            }

            _text.Draw(s, position);

            return false;
        }
        private bool DrawButton(int x, int y, int index)
        {
            var r = false;

            var sRect = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            var dRect = new Rectangle(x, y, 32, 32);

            if (dRect.Contains(_mouseInput.Position))
            {
                dRect.X -= 1;
                dRect.Y -= 1;
                dRect.Width += 2;
                dRect.Height += 2;

                if (_mouseInput.LeftButtonClick)
                    r = true;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Icons, dRect, sRect, Color.White);
            _spriteBatch.End();

            return r;
        }
        // ReSharper restore UnusedMember.Local
    }
}
