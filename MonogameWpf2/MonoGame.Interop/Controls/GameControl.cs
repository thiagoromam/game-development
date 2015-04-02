using System;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Interop.Arguments;
using MonoGame.Interop.Module;
using MonoGame.Interop.Services;

namespace MonoGame.Interop.Controls
{
    public sealed class GameControl : ContentControl
    {
        public static readonly DependencyProperty GameModuleProperty;
        private IGameModule _game;
        private IInternalGameModule _internalModule;
        private DrawingSurface _drawingSurface;

        static GameControl()
        {
            GameModuleProperty = DependencyProperty.Register(
                "GameModule",
                typeof(IGameModule),
                typeof(GameControl),
                new PropertyMetadata(default(IGameModule), (s, e) => ((GameControl)s).OnGameModuleChanged()));
        }

        public IGameModule GameModule
        {
            get { return (IGameModule)GetValue(GameModuleProperty); }
            set { SetValue(GameModuleProperty, value); }
        }

        private void OnGameModuleChanged()
        {
            if (_game != null)
                throw new InvalidOperationException();

            _game = GameModule;
            _internalModule = (IInternalGameModule)GameModule;

            _drawingSurface = new DrawingSurface();
            _drawingSurface.Loaded += OnDrawingSurfaceLoaded;
            _drawingSurface.Unloaded += OnDrawingSurfaceUnloaded;

            _drawingSurface.LoadContent += OnLoadContent;
            _drawingSurface.Draw += OnDraw;
            _drawingSurface.MouseMove += OnMouseMove;
            _drawingSurface.MouseDown += OnMouseDown;

            KeyDown += OnKeyDown;
            Content = _drawingSurface;
        }

        private void OnDrawingSurfaceLoaded(object sender, RoutedEventArgs e)
        {
            if (!_internalModule.IsRunning)
                _internalModule.Run();
        }
        private void OnDrawingSurfaceUnloaded(object sender, RoutedEventArgs e)
        {
            if (_internalModule.IsRunning)
                _internalModule.Stop();
        }

        private void OnLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            var container = new ServiceContainer();
            container.AddService(typeof(IGraphicsDeviceService), MonoGameInteropInjection.Container.Resolve<GraphicsDeviceService>());
            container.AddService(typeof(GraphicsDevice), e.GraphicsDevice);

            _internalModule.Prepare(_drawingSurface, container);
            _game.Initialize();
            _game.LoadContent();
            _internalModule.Run();
        }
        private void OnDraw(object sender, DrawEventArgs e)
        {
            _internalModule.Draw(e);
        }

        // events
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _game.OnMouseMove(new GameMouseEventArgs(e, _drawingSurface));
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _game.OnMouseDown(new GameMouseButtonEventArgs(e, _drawingSurface));
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _game.OnKeyDown(e);
        }
    }
}
