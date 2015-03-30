using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameWpf2.Controls;
using MonogameWpf2.Services;

namespace MonogameWpf2
{
    public partial class MainWindow
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private Texture2D _texture;
        private Rectangle _source;
        private Vector2 _origin;
        private List<Vector2> _positions;

        public MainWindow()
        {
            InitializeComponent();

            _positions = new List<Vector2>();
        }

        private void OnLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            _spriteBatch = new SpriteBatch(e.GraphicsDevice);

            var container = new ServiceContainer();
            container.AddService(typeof(IGraphicsDeviceService), Injection.Container.Resolve<GraphicsDeviceService>());
            _content = new ContentManager(container, "Content");

            _texture = _content.Load<Texture2D>("smurf_sprite");
            _source = new Rectangle(31, 4, 71, 120);
            _origin = new Vector2(_source.Width, _source.Height) / 2;

            _positions = new List<Vector2> { Vector2.Zero };
        }
        private void OnDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            foreach (var position in _positions)
                _spriteBatch.Draw(_texture, position, _source, Color.White, 0, _origin, 1, 0, 0);
            
            _spriteBatch.End();

            GraphicsControl.Invalidate();
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _positions[_positions.Count - 1] = GetMousePosition(e);
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _positions.Insert(0, GetMousePosition(e));
        }

        private Vector2 GetMousePosition(MouseEventArgs e)
        {
            var position = e.GetPosition(GraphicsControl);
            return new Vector2((float) position.X, (float) position.Y);
        }

        private void OnFlyoutClick(object sender, RoutedEventArgs e)
        {
            Flyout.IsOpen = ((ToggleButton)sender).IsChecked ?? false;
        }
    }
}
