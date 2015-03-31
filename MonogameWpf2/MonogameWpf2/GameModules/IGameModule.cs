using System;
using System.Windows.Input;
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
        void Draw(DrawEventArgs e);

        // events
        void OnMouseMove(GameMouseEventArgs e);
        void OnMouseDown(GameMouseButtonEventArgs e);
        void OnKeyDown(KeyEventArgs e);
    }

    public abstract class BaseGameModule : IGameModule
    {
        private readonly string _contentDirectory;

        protected BaseGameModule(string contentDirectory)
        {
            _contentDirectory = contentDirectory;
        }

        public SpriteBatch SpriteBatch { get; private set; }
        public ContentManager Content { get; private set; }

        void IGameModule.Initialize(IServiceProvider provider)
        {
            SpriteBatch = new SpriteBatch((GraphicsDevice)provider.GetService(typeof(GraphicsDevice)));
            Content = new ContentManager(provider, _contentDirectory);
        }
        public abstract void LoadContent();
        public abstract void Draw(DrawEventArgs e);

        // events
        public virtual void OnMouseMove(GameMouseEventArgs e) { }
        public virtual void OnMouseDown(GameMouseButtonEventArgs e) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
    }
}