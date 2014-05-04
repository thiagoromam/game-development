using MapEditor.Helpers;
using MapEditor.MapClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    public class GameRoot : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Text _text;
        private Map _map;
        private MouseControl _mouseControl;

        private int _currentLayer = 1;
        private int _mouseDragSegment = -1;
        private readonly string[] _mapLayers = { "back", "mid", "fore", "map" };
        private readonly float[] _layersScalar = { 0.375f, 0.5f, 0.625f };
        private readonly Color _gridColor = new Color(255, 0, 0, 100);
        private Vector2 _scroll;
        private DrawingMode _drawingMode;
        private readonly string[] _drawingLayers = { "select", "colision" };
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
            _map = new Map();
            _mouseControl = new MouseControl();
            _drawingMode = DrawingMode.SegmentSelection;
            _editArea = new AreaRectangle(100, 50, 400, 450, new Color(255, 255, 255, 100));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);

            _text = new Text(Art.Arial, _spriteBatch, _mouseControl);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _mouseControl.Update();

            if (CanEdit())
            {
                if (_drawingMode == DrawingMode.SegmentSelection)
                {
                    if (_mouseControl.LeftButtonPressed)
                    {
                        var f = _map.GetHoveredSegment(_currentLayer, _scroll, _mouseControl.Position);

                        if (f != -1)
                            _mouseDragSegment = f;
                    }
                }
                else if (_drawingMode == DrawingMode.CollisionMap)
                {
                    var x = (int)(_mouseControl.Position.X + _scroll.X / 2) / 32;
                    var y = (int)(_mouseControl.Position.Y + _scroll.Y / 2) / 32;

                    if (x.Between(0, _map.MaxGridDimension0Index) && y.Between(0, _map.MaxGridDimension1Index))
                    {
                        if (_mouseControl.LeftButtonClick)
                            _map.Grid[x, y] = 1;
                        else if (_mouseControl.RightButtonClick)
                            _map.Grid[x, y] = 0;
                    }
                }
            }

            if (_mouseDragSegment > -1)
            {
                if (!_mouseControl.LeftButtonPressed)
                    _mouseDragSegment = -1;
                else
                    _map.Segments[_currentLayer, _mouseDragSegment].Location += _mouseControl.Position - _mouseControl.PreviousPosition;
            }

            if (_mouseControl.MiddleButtonPressed)
            {
                _scroll -= (_mouseControl.Position - _mouseControl.PreviousPosition) * 2;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _map.Draw(_spriteBatch, _scroll);

            if (_drawingMode == DrawingMode.SegmentSelection)
                DrawMapSegments();

            DrawGrid();
            _editArea.Draw(_spriteBatch);
            DrawText();

            _mouseControl.Draw(_spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawText()
        {
            if (_text.DrawClickable(_mapLayers[_currentLayer], new Vector2(5, 5)))
                _currentLayer = (_currentLayer + 1) % 3;

            if (_text.DrawClickable(_drawingLayers[(int)_drawingMode], new Vector2(5, 25)))
                _drawingMode = (DrawingMode)((int)(_drawingMode + 1) % 2);
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

                if (_mouseControl.LeftButtonPressed)
                {
                    if (_mouseControl.Position.X > destination.X &&
                        _mouseControl.Position.X < 780 &&
                        _mouseControl.Position.Y > destination.Y &&
                        _mouseControl.Position.Y < destination.Y + 45)
                    {
                        if (_mouseDragSegment == -1)
                        {
                            var f = _map.AddSegment(_currentLayer, i);
                            if (f <= -1)
                                continue;

                            var mapSegment = _map.Segments[_currentLayer, f];
                            mapSegment.Location = _mouseControl.Position - segment.SourceLength / 4 + _scroll * _layersScalar[_currentLayer];
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

        private bool CanEdit()
        {
            return _editArea.Area.Contains(_mouseControl.Position);
        }
    }
}
