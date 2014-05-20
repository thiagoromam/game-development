using Funq.Fast;
using KeyboardLib.Api;
using MapEditor.Editor;
using Helpers;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MouseLib.Api;
using SharedLib;
using SharedLib.Mouse;
using TextLib.Api;

// ReSharper disable ForCanBeConvertedToForeach

namespace MapEditor
{
    public class GameRoot : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ISettings _settings;
        private IMouseComponent _mouseComponent;
        private IMouseInput _mouseInput;
        private IMouseDrawer _mouseDrawer;
        private IKeyboardComponent _keyboardComponent;

        private IText _text;
        private IMapComponent _map;

        private EditorGuiManager _guiManager;

        private int _mouseDragSegment = -1;
        private readonly float[] _layersScalar = { 0.375f, 0.5f, 0.625f };
        private readonly Color _gridColor = new Color(255, 0, 0, 100);
        private Vector2 _scroll;
        private AreaRectangle _editArea;

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
            _editArea = new AreaRectangle(100, 50, 400, 450, new Color(255, 255, 255, 100));

            App.Register();
            _mouseComponent = DependencyInjection.Resolve<IMouseComponent>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _mouseDrawer = DependencyInjection.Resolve<IMouseDrawer>();
            _settings = DependencyInjection.Resolve<ISettings>();
            _map = DependencyInjection.Resolve<IMapComponent>();
            _keyboardComponent = DependencyInjection.Resolve<IKeyboardComponent>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            App.Register(_spriteBatch);

            Art.LoadContent(Content);
            SharedArt.LoadContent(Content);
            LibContent.SetContents();

            _text = DependencyInjection.Resolve<IText>();
            _guiManager = new EditorGuiManager();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _mouseComponent.Update();
            UpdateEdition();
            UpdateDragSegment();
            UpdateScroll();

            _keyboardComponent.Update();
            _guiManager.Update();

            base.Update(gameTime);
        }
        private void UpdateEdition()
        {
            if (!CanEdit()) return;
            
            switch (_settings.CurrentDrawingMode)
            {
                case DrawingMode.SegmentSelection:
                    CheckSegmentUpdates();
                    break;
                case DrawingMode.CollisionMap:
                    CheckCollisionMapUpdates();
                    break;
                case DrawingMode.Ledge:
                    CheckLedgesUpdates();
                    break;
            }
        }
        private bool CanEdit()
        {
            return _editArea.Area.Contains(_mouseInput.Position);
        }
        private void CheckSegmentUpdates()
        {
            if (!_mouseInput.LeftButtonDown) return;

            var f = _map.GetHoveredSegment(_settings.CurrentMapLayer, _scroll, _mouseInput.Position);
            if (f != -1)
                _mouseDragSegment = f;
        }
        private void CheckCollisionMapUpdates()
        {
            if (!_mouseInput.LeftButtonClick && !_mouseInput.RightButtonClick) return;

            var x = (int)(_mouseInput.Position.X + _scroll.X / 2) / 32;
            var y = (int)(_mouseInput.Position.Y + _scroll.Y / 2) / 32;

            if (!x.Between(0, _map.MaxGridDimension0Index) || !y.Between(0, _map.MaxGridDimension1Index)) return;

            if (_mouseInput.LeftButtonClick)
                _map.Grid[x, y] = 1;
            else if (_mouseInput.RightButtonClick)
                _map.Grid[x, y] = 0;
        }
        private void CheckLedgesUpdates()
        {
            if (!_mouseInput.LeftButtonClick) return;

            var ledge = _map.Ledges[_settings.SelectedLedge];

            if (ledge.TotalNodes >= 15) return;

            ledge.Nodes[ledge.TotalNodes] = _mouseInput.Position + _scroll / 2;
            ledge.TotalNodes++;
        }
        private void UpdateDragSegment()
        {
            if (_mouseDragSegment <= -1) return;

            if (!_mouseInput.LeftButtonPressed)
                _mouseDragSegment = -1;
            else
                _map.Segments[_settings.CurrentMapLayer, _mouseDragSegment].Location += _mouseInput.Position - _mouseInput.PreviousPosition;
        }
        private void UpdateScroll()
        {
            if (_mouseInput.MiddleButtonPressed)
                _scroll -= (_mouseInput.Position - _mouseInput.PreviousPosition)*2;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _map.Draw(_spriteBatch, _scroll);
            
            DrawGrid();

            if (_settings.CurrentDrawingMode == DrawingMode.SegmentSelection)
                DrawMapSegments();

            _editArea.Draw(_spriteBatch);
            DrawLedges();
            _guiManager.Draw(_spriteBatch);

            _mouseDrawer.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
        private void DrawMapSegments()
        {
            var destination = new Rectangle();

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(500, 20, 280, 550), new Color(0, 0, 0, 100));
            _spriteBatch.End();

            for (var i = 0; i < 9; i++)
            {
                var segment = _map.Definitions[i];
                if (segment == null)
                    continue;

                _spriteBatch.Begin();

                destination.X = 500;
                destination.Y = 50 + i * 60;

                if (segment.Source.Width > segment.Source.Height)
                {
                    destination.Width = 45;
                    destination.Height = (int)(segment.Source.Height / (float)segment.Source.Width * 45.0f);
                }
                else
                {
                    destination.Height = 45;
                    destination.Width = (int)(segment.Source.Width / (float)segment.Source.Height * 45.0f);
                }

                _spriteBatch.Draw(Art.Maps[segment.Index], destination, segment.Source, Color.White);
                _spriteBatch.End();

                _text.Draw(segment.Name, new Vector2(destination.X + 50, destination.Y));

                if (_mouseInput.LeftButtonPressed)
                {
                    if (_mouseInput.Position.X > destination.X &&
                        _mouseInput.Position.X < 780 &&
                        _mouseInput.Position.Y > destination.Y &&
                        _mouseInput.Position.Y < destination.Y + 45)
                    {
                        if (_mouseDragSegment == -1)
                        {
                            var f = _map.AddSegment(_settings.CurrentMapLayer, i);
                            if (f <= -1)
                                continue;

                            var mapSegment = _map.Segments[_settings.CurrentMapLayer, f];
                            mapSegment.Location = _mouseInput.Position - segment.SourceLength / 4 + _scroll * _layersScalar[_settings.CurrentMapLayer];
                            _mouseDragSegment = f;
                        }
                    }
                }
            }
        }
        private void DrawGrid()
        {
            _spriteBatch.Begin();

            for (var x = 0; x <= _map.MaxGridDimension0Index; x++)
            {
                for (var y = 0; y <= _map.MaxGridDimension1Index; y++)
                {
                    Rectangle destination;
                    destination.X = x * 32 - (int)(_scroll.X / 2);
                    destination.Y = y * 32 - (int)(_scroll.Y / 2);

                    var xIsLessThanMaxIndice = x < _map.MaxGridDimension0Index;
                    var yIsLessThanMaxIndice = y < _map.MaxGridDimension1Index;

                    if (xIsLessThanMaxIndice)
                    {
                        destination.Width = 32;
                        destination.Height = 1;
                        _spriteBatch.Draw(SharedArt.Null, destination, _gridColor);
                    }

                    if (yIsLessThanMaxIndice)
                    {
                        destination.Width = 1;
                        destination.Height = 32;
                        _spriteBatch.Draw(SharedArt.Null, destination, _gridColor);
                    }

                    if (xIsLessThanMaxIndice && yIsLessThanMaxIndice && _map.Grid[x, y] == 1)
                    {
                        destination.Width = 32;
                        destination.Height = 32;
                        _spriteBatch.Draw(SharedArt.Null, destination, _gridColor);
                    }
                }
            }



            _spriteBatch.End();
        }
        private void DrawLedges()
        {
            var rectangle = new Rectangle(32, 0, 32, 32);
            var softColor = new Color(255, 255, 255, 75);
            var hardColor = new Color(255, 0, 0, 75);

            _spriteBatch.Begin();

            for (var i = 0; i < _map.Ledges.Length; ++i)
            {
                var ledge = _map.Ledges[i];

                for (var n = 0; n < ledge.TotalNodes; ++n)
                {
                    var tVector = ledge.Nodes[n] - _scroll / 2;
                    tVector.X -= 5;
                    var iColor = _settings.SelectedLedge == i ? Color.Yellow : Color.White;

                    _spriteBatch.Draw(SharedArt.Icons, tVector, rectangle, iColor, 0, Vector2.Zero, 0.35f, SpriteEffects.None, 0);

                    if (n < ledge.TotalNodes - 1)
                    {
                        var nVector = ledge.Nodes[n + 1] - _scroll / 2;
                        var iVectorPart = nVector - tVector;

                        nVector.X -= 4;

                        for (var x = 0; x < 20; x++)
                        {
                            var xVector = iVectorPart * (x / 20f) + tVector;
                            var xColor = ledge.Flags == 0 ? softColor : hardColor;
                            _spriteBatch.Draw(SharedArt.Icons, xVector, rectangle, xColor, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 0);
                        }
                    }
                }
            }

            _spriteBatch.End();
        }
    }
}
