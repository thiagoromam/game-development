using System;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameWpf2.Arguments;
using MonogameWpf2.Controls;

namespace MonogameWpf2.Module
{
    public abstract class BaseGameModule : IGameModule, IInternalGameModule
    {
        private readonly string _contentDirectory;
        private GameUpdater _updater;
        private DrawingSurface _drawingSurface;

        protected BaseGameModule(string contentDirectory)
        {
            _contentDirectory = contentDirectory;
        }

        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected ContentManager Content { get; private set; }
        protected double Width
        {
            get { return _drawingSurface.Width; }
            set { _drawingSurface.Width = value; }
        }
        protected double Height
        {
            get { return _drawingSurface.Height; }
            set { _drawingSurface.Height = value; }
        }

        void IInternalGameModule.Prepare(DrawingSurface drawingSurface, IServiceProvider provider)
        {
            _drawingSurface = drawingSurface;
            GraphicsDevice = drawingSurface.GraphicsDevice;
            SpriteBatch = new SpriteBatch((GraphicsDevice)provider.GetService(typeof(GraphicsDevice)));
            Content = new ContentManager(provider, _contentDirectory);
            _updater = new GameUpdater(Update, drawingSurface.Invalidate);
        }
        void IInternalGameModule.Run()
        {
            _updater.Start();
        }
        void IInternalGameModule.Draw(DrawEventArgs e)
        {
            Draw();
            _updater.Drawing = false;
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw() { }

        // events
        public virtual void OnMouseMove(GameMouseEventArgs e) { }
        public virtual void OnMouseDown(GameMouseButtonEventArgs e) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
    }
}