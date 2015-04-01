using System;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameWpf2.Controls;
using MonogameWpf2.Util;

namespace MonogameWpf2.GameModules
{
    public interface IGameModule
    {
        void Initialize(IServiceProvider provider);
        void LoadContent();
        void Run();
        void Draw(DrawEventArgs e);

        // events
        void OnMouseMove(GameMouseEventArgs e);
        void OnMouseDown(GameMouseButtonEventArgs e);
        void OnKeyDown(KeyEventArgs e);
    }

    public abstract class BaseGameModule : IGameModule
    {
        private readonly string _contentDirectory;
        private readonly GameUpdater _updater;

        protected BaseGameModule(DrawingSurface drawingSurface, string contentDirectory)
        {
            GraphicsDevice = drawingSurface.GraphicsDevice;
            _contentDirectory = contentDirectory;
            _updater = new GameUpdater(Update, drawingSurface.Invalidate);
        }

        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected ContentManager Content { get; private set; }

        void IGameModule.Initialize(IServiceProvider provider)
        {
            SpriteBatch = new SpriteBatch((GraphicsDevice)provider.GetService(typeof(GraphicsDevice)));
            Content = new ContentManager(provider, _contentDirectory);
        }
        public abstract void LoadContent();
        void IGameModule.Run()
        {
            _updater.Start();
        }
        protected virtual void Update(GameTime gameTime) { }
        void IGameModule.Draw(DrawEventArgs e)
        {
            Draw();
            _updater.Drawing = false;
        }
        protected abstract void Draw();

        // events
        public virtual void OnMouseMove(GameMouseEventArgs e) { }
        public virtual void OnMouseDown(GameMouseButtonEventArgs e) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
    }
}