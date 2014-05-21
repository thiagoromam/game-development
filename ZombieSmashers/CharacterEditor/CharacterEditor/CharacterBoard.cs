using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using SharedLib;

namespace CharacterEditor
{
    public class CharacterBoard
    {
        private const int FaceLeft = 0;
        private const int FaceRight = 1;

        private readonly CharacterDefinition _characterDefinition;
        private readonly IReadonlySettings _settings;
        private readonly IMouseInput _mouseInput;
        private readonly AreaRectangle _editingArea;

        public CharacterBoard()
        {
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _settings = DependencyInjection.Resolve<IReadonlySettings>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _editingArea = new AreaRectangle(0, 0, 575, 450, Color.White);
        }

        public void Update()
        {
            if (!CanEdit())
                return;

            var mouseDiference = _mouseInput.Position - _mouseInput.PreviousPosition;
            UpdateDrag(mouseDiference);
            UpdateRotation(mouseDiference);
            UpdateScaling(mouseDiference);
        }
        private void UpdateDrag(Vector2 mouseDiference)
        {
            if (_mouseInput.LeftButtonPressed)
                _settings.SelectedPart.Location += mouseDiference / 2;
        }
        private void UpdateRotation(Vector2 mouseDiference)
        {
            if (_mouseInput.RightButtonPressed)
                _settings.SelectedPart.Rotation += mouseDiference.Y  / 100;
        }
        private void UpdateScaling(Vector2 mouseDiference)
        {
            if (_mouseInput.MiddleButtonPressed)
                _settings.SelectedPart.Scaling += mouseDiference * 0.01f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _editingArea.Draw(spriteBatch);
            DrawCharacter(spriteBatch, new Vector2(400, 450), 2f, FaceRight, _settings.SelectedFrameIndex, false, 1);
        }
        private void DrawCharacter(SpriteBatch spriteBatch, Vector2 loc, float scale, int face, int frameIndex, bool preview, float alpha)
        {
            var source = new Rectangle();

            var frame = _characterDefinition.Frames[frameIndex];

            spriteBatch.Begin();

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

                var location = part.Location * scale + loc;
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

                spriteBatch.Draw(
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

            spriteBatch.End();
        }

        private bool CanEdit()
        {
            return _editingArea.Area.Contains(_mouseInput.Position);
        }
    }
}