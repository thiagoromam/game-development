using CharacterEditor.Character;
using CharacterEditor.Ioc;
using CharacterEditor.Ioc.Api.Editor;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Helpers;
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

        private const int FaceLeft = 0;
        private const int FaceRight = 1;

        private CharacterDefinition _characterDefinition;
        private IMouseComponent _mouseComponent;
        private IMouseDrawer _mouseDrawer;
        private IMouseInput _mouseInput;
        private IIconsPalleteComponent _iconsIconsPallete;
        private ISettings _settings;
        private IText _text;
        private ITextContent _textContent;
        private IPartsPalleteComponent _partsPallete;

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

            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _mouseComponent = DependencyInjection.Resolve<IMouseComponent>();
            _mouseDrawer = DependencyInjection.Resolve<IMouseDrawer>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _iconsIconsPallete = DependencyInjection.Resolve<IIconsPalleteComponent>();
            _settings = DependencyInjection.Resolve<ISettings>();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.LoadContent(Content);
            SharedArt.LoadContent(Content);

            App.Register(_spriteBatch);
            LibContent.SetContents();

            _text = DependencyInjection.Resolve<IText>();
            _partsPallete = DependencyInjection.Resolve<IPartsPalleteComponent>();
            _textContent = DependencyInjection.Resolve<ITextContent>();
            _textContent.Size = 0.75f;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _mouseComponent.Update();
            _iconsIconsPallete.Update();
            _partsPallete.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Null, new Rectangle(590, 0, 300, 600), new Color(0, 0, 0, 0.5f));
            _spriteBatch.End();

            _iconsIconsPallete.Draw(_spriteBatch);
            _partsPallete.Draw(_spriteBatch);
            //DrawPartsList();
            DrawCharacter(new Vector2(400f, 450f), 2f, FaceRight, _settings.SelectedFrameIndex, false, 1);
            _mouseDrawer.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
        private void DrawCharacter(Vector2 loc, float scale, int face, int frameIndex, bool preview, float alpha)
        {
            var source = new Rectangle();

            var frame = _characterDefinition.Frames[frameIndex];

            _spriteBatch.Begin();

            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index == -1) continue;

                source.X = ((part.Index % 64) % 5) * 64;
                source.Y = ((part.Index % 64) / 5) * 64;
                source.Width = 64;
                source.Height = 64;

                if (part.Index >= 192)
                {
                    source.X = ((part.Index % 64) % 3) * 80;
                    source.Width = 80;
                }

                var location = part.Location * scale * loc;
                var scaling = part.Scaling * scale;
                if (part.Index >= 128)
                    scaling *= 1.35f;

                var rotation = part.Rotation;
                if (face == FaceLeft)
                {
                    rotation = -rotation;
                    location.X -= part.Location.X * scale * 2;
                }

                Texture2D texture = null;

                switch (part.Index / 64)
                {
                    case 0: texture = Art.Heads[_characterDefinition.HeadIndex]; break;
                    case 1: texture = Art.Torsos[_characterDefinition.TorsoIndex]; break;
                    case 2: texture = Art.Legs[_characterDefinition.LegsIndex]; break;
                    case 3: texture = Art.Weapons[_characterDefinition.WeaponIndex]; break;
                }

                var color = new Color(255, 255, 255, alpha * 255);

                if (!preview && _settings.SelectedPartIndex == i)
                {
                    color.G = 0;
                    color.B = 0;
                }

                var flip = face == FaceRight && part.Flip == 0 || face == FaceLeft && part.Flip == 1;

                if (texture == null) continue;
                var origin = new Vector2(source.Width / 2f, 32f);

                _spriteBatch.Draw(
                    texture,
                    location,
                    source,
                    color,
                    rotation,
                    origin,
                    scaling,
                    flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    1
                );
            }

            _spriteBatch.End();
        }
        private void DrawPartsList()
        {
            for (var i = 0; i < _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts.Length; i++)
            {
                var y = 5 + i * 15;

                string line;

                var index = _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[i].Index;
                if (index < 0)
                    line = "";
                else if (index < 64)
                    line = "head" + index;
                else if (index < 74)
                    line = "torso" + index;
                else if (index < 128)
                    line = "arms" + index;
                else if (index < 192)
                    line = "legs" + index;
                else
                    line = "weapon" + index;

                if (_settings.SelectedPartIndex == i)
                {
                    _text.Draw(i + ": " + line, new Vector2(600, y), Color.Lime);

                    if (DrawButton(700, y, 1))
                    {
                        SwapParts(_settings.SelectedPartIndex, _settings.SelectedPartIndex - 1);
                        if (_settings.SelectedPartIndex > 0) _settings.SelectedPartIndex--;
                    }

                    if (DrawButton(720, y, 2))
                    {
                        SwapParts(_settings.SelectedPartIndex, _settings.SelectedPartIndex + 1);
                        if (_settings.SelectedPartIndex < _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts.Length - 1)
                            _settings.SelectedPartIndex++;
                    }

                    var part = _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[_settings.SelectedPartIndex];
                    if (DrawClickText(740, y, (part.Flip == 0 ? "(n)" : "(m)")))
                    {
                        part.Flip = 1 - part.Flip;
                    }

                    if (DrawClickText(762, y, "(r)"))
                        part.Scaling = new Vector2(1.0f, 1.0f);

                    if (DrawClickText(780, y, "(x)"))
                        part.Index = -1;
                }
                else
                {
                    if (DrawClickText(600, y, i + ": " + line))
                        _settings.SelectedPartIndex = i;
                }
            }
        }

        private void SwapParts(int idx1, int idx2)
        {
            if (idx1 < 0 || idx2 < 0 ||
                idx1 >= _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts.Length ||
                idx2 >= _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts.Length)
                return;

            var i = _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[idx1];
            var j = _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[idx2];

            _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[idx1] = j;
            _characterDefinition.Frames[_settings.SelectedFrameIndex].Parts[idx2] = i;
        }

        // Suport
        private bool DrawButton(int x, int y, int index)
        {
            var r = false;

            var sRect = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            var dRect = new Rectangle(x, y, 32, 32);

            if (dRect.Contains(_mouseInput.Position))
            {
                dRect.X -= 1;
                dRect.Y -= 1;
                dRect.Width += 2;
                dRect.Height += 2;

                if (_mouseInput.LeftButtonClick)
                    r = true;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(SharedArt.Icons, dRect, sRect, Color.White);
            _spriteBatch.End();

            return r;
        }
        public bool DrawClickText(int x, int y, string s)
        {
            var position = new Vector2(x, y);

            if (_text.MouseIntersects(s, position))
            {
                _text.Draw(s, position, Color.Yellow);
                return _mouseInput.LeftButtonClick;
            }

            _text.Draw(s, position);

            return false;
        }
    }
}
