using MapEditor.Editor;
using MapEditor.Helpers;
using MapEditor.Input;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Input;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using MapEditor.MapClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private KeyboardInput _keyboardInput;

        private Text _text;
        private IMapComponent _map;

        private GuiManager _guiManager;

        private int _mouseDragSegment = -1;
        private readonly float[] _layersScalar = { 0.375f, 0.5f, 0.625f };
        private readonly Color _gridColor = new Color(255, 0, 0, 100);
        private Vector2 _scroll;
        private AreaRectangle _editArea;
        private int _currentLedge;

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
            _keyboardInput = new KeyboardInput();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);
            App.Register(_spriteBatch);

            _text = new Text(Art.Arial, _spriteBatch);
            _guiManager = new GuiManager();
            
            _mouseComponent = App.Container.Resolve<IMouseComponent>();
            _mouseInput = App.Container.Resolve<IMouseInput>();
            _settings = App.Container.Resolve<ISettings>();
            _map = App.Container.Resolve<IMapComponent>();
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

            if (CanEdit())
            {
                switch (_settings.DrawingMode)
                {
                    case DrawingMode.SegmentSelection: CheckSegmentUpdates(); break;
                    case DrawingMode.CollisionMap: CheckCollisionMapUpdates(); break;
                    case DrawingMode.Ledge: CheckLedgesUpdates(); break;
                }
            }

            if (_mouseDragSegment > -1)
            {
                if (!_mouseInput.LeftButtonPressed)
                    _mouseDragSegment = -1;
                else
                    _map.Segments[_settings.MapLayer, _mouseDragSegment].Location += _mouseInput.Position - _mouseInput.PreviousPosition;
            }

            if (_mouseInput.MiddleButtonPressed)
            {
                _scroll -= (_mouseInput.Position - _mouseInput.PreviousPosition) * 2;
            }

            _keyboardInput.Update();
            _guiManager.Update();

            base.Update(gameTime);
        }
        private bool CanEdit()
        {
            return _editArea.Area.Contains(_mouseInput.Position);
        }
        private void CheckSegmentUpdates()
        {
            if (!_mouseInput.LeftButtonPressed) return;

            var f = _map.GetHoveredSegment(_settings.MapLayer, _scroll, _mouseInput.Position);
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

            var ledge = _map.Ledges[_currentLedge];

            if (ledge.TotalNodes >= 15) return;

            ledge.Nodes[ledge.TotalNodes] = _mouseInput.Position + _scroll / 2;
            ledge.TotalNodes++;
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _map.Draw(_spriteBatch, _scroll);
            
            DrawGrid();

            switch (_settings.DrawingMode)
            {
                case DrawingMode.SegmentSelection: DrawMapSegments(); break;
                case DrawingMode.Ledge: DrawLedgePallete(); break;
            }

            _editArea.Draw(_spriteBatch);
            DrawLedges();
            _guiManager.Draw(_spriteBatch);

            _mouseComponent.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
        private void DrawMapSegments()
        {
            var destination = new Rectangle();

            _spriteBatch.Begin();
            _spriteBatch.Draw(Art.Null, new Rectangle(500, 20, 280, 550), new Color(0, 0, 0, 100));
            _spriteBatch.End();

            _text.Size = 0.8f;
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

                _text.Color = Color.White;

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
                            var f = _map.AddSegment(_settings.MapLayer, i);
                            if (f <= -1)
                                continue;

                            var mapSegment = _map.Segments[_settings.MapLayer, f];
                            mapSegment.Location = _mouseInput.Position - segment.SourceLength / 4 + _scroll * _layersScalar[_settings.MapLayer];
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
                        _spriteBatch.Draw(Art.Null, destination, _gridColor);
                    }

                    if (yIsLessThanMaxIndice)
                    {
                        destination.Width = 1;
                        destination.Height = 32;
                        _spriteBatch.Draw(Art.Null, destination, _gridColor);
                    }

                    if (xIsLessThanMaxIndice && yIsLessThanMaxIndice && _map.Grid[x, y] == 1)
                    {
                        destination.Width = 32;
                        destination.Height = 32;
                        _spriteBatch.Draw(Art.Null, destination, _gridColor);
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
                    var iColor = _currentLedge == i ? Color.Yellow : Color.White;

                    _spriteBatch.Draw(Art.Icons, tVector, rectangle, iColor, 0, Vector2.Zero, 0.35f, SpriteEffects.None, 0);

                    if (n < ledge.TotalNodes - 1)
                    {
                        var nVector = ledge.Nodes[n + 1] - _scroll / 2;
                        var iVectorPart = nVector - tVector;

                        nVector.X -= 4;

                        for (var x = 0; x < 20; x++)
                        {
                            var xVector = iVectorPart * (x / 20f) + tVector;
                            var xColor = ledge.Flags == 0 ? softColor : hardColor;
                            _spriteBatch.Draw(Art.Icons, xVector, rectangle, xColor, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 0);
                        }
                    }
                }
            }

            _spriteBatch.End();
        }
        private void DrawLedgePallete()
        {
            for (var i = 0; i < _map.Ledges.Length; i++)
            {
                var ledge = _map.Ledges[i];

                var textPosition = new Vector2(520, 50 + i * 20);
                var text = "ledge " + i;
                if (_currentLedge == i)
                {
                    _text.Color = Color.Lime;
                    _text.Draw(text, textPosition);
                    _text.Color = Color.White;
                }
                else
                {
                    if (_text.DrawClickable(text, textPosition))
                        _currentLedge = i;
                }

                textPosition.X = 620;
                _text.Draw("n" + ledge.TotalNodes, textPosition);

                textPosition.X = 680;
                if (_text.DrawClickable("f" + ledge.Flags, textPosition))
                    ledge.Flags = (ledge.Flags + 1) % 2;
            }
        }
    }
}
