using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Graphics;
using MonogameWpf2.Controls;
using MonogameWpf2.GameModules;
using MonogameWpf2.Services;
using MonogameWpf2.Util;

namespace MonogameWpf2
{
    public partial class MainWindow
    {
        private IGameModule _gameModule;

        public MainWindow()
        {
            InitializeComponent();

            KeyDown += OnKeyDown;
        }

        private void OnLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            var container = new ServiceContainer();
            container.AddService(typeof(IGraphicsDeviceService), Injection.Container.Resolve<GraphicsDeviceService>());
            container.AddService(typeof(GraphicsDevice), e.GraphicsDevice);

            _gameModule = new MainGameModule(GraphicsControl);
            _gameModule.Initialize(container);
            _gameModule.LoadContent();
        }
        private void OnDraw(object sender, DrawEventArgs e)
        {
            _gameModule.Draw(e);
        }

        // events
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _gameModule.OnMouseMove(new GameMouseEventArgs(e, GraphicsControl));
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _gameModule.OnMouseDown(new GameMouseButtonEventArgs(e, GraphicsControl));
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _gameModule.OnKeyDown(e);
        }

        private void OnGeminiUrlRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
