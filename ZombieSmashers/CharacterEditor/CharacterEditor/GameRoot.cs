using CharacterEditor.Board;
using CharacterEditor.Editor;
using CharacterEditor.Ioc;
using Funq.Fast;
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
        private IKeyboardComponent _keyboardComponent;
        private IKeyboardControl _keyboardControl;
        private ITextContent _textContent;
        private BoardManager _boardManager;
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
            BoardExtensions.Initialize();

            _mouseComponent = DependencyInjection.Resolve<IMouseComponent>();
            _mouseDrawer = DependencyInjection.Resolve<IMouseDrawer>();
            _keyboardComponent = DependencyInjection.Resolve<IKeyboardComponent>();
            _keyboardControl = DependencyInjection.Resolve<IKeyboardControl>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);
            SharedArt.LoadContent(Content);

            App.Register(_spriteBatch);
            LibContent.SetContents();
            LegacySuport.Load(_spriteBatch);

            _textContent = DependencyInjection.Resolve<ITextContent>();
            _textContent.Size = 0.75f;

            _guiManager = new GuiManager();
            _boardManager = new BoardManager();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_keyboardControl.EditingMode && _keyboardControl.IsKeyPressed(Keys.Escape))
                Exit();

            _keyboardComponent.Update();
            _mouseComponent.Update();
            _guiManager.Update();
            _boardManager.Update(gameTime);

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
            _boardManager.Draw(_spriteBatch);
            _mouseDrawer.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
