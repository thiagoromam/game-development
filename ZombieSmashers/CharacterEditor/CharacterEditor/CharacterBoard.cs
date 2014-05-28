using System;
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
        private readonly IReadOnlySettings _settings;
        private readonly IMouseInput _mouseInput;
        private readonly AreaRectangle _editingArea;
        private readonly Vector2 _characterPosition;
        private readonly Vector2 _animationPosition;
        private bool _playing;
        private float _currentFrameTime;
        private int _currentKeyFrameIndex;

        public CharacterBoard()
        {
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _editingArea = new AreaRectangle(205, 110, 380, 345, new Color(10, 10, 10, 50));

            _characterPosition = new Vector2(400, 450);
            _animationPosition = new Vector2(500, 100);
        }

        public void Update(GameTime gameTime)
        {
            if (!CanEdit())
            {
                var mouseDiference = _mouseInput.Position - _mouseInput.PreviousPosition;
                UpdateDrag(mouseDiference);
                UpdateRotation(mouseDiference);
                UpdateScaling(mouseDiference);
            }

            UpdateAnimation(gameTime);
        }
        private void UpdateDrag(Vector2 mouseDiference)
        {
            if (_mouseInput.LeftButtonPressed)
                _settings.SelectedPart.Location += mouseDiference / 2;
        }
        private void UpdateRotation(Vector2 mouseDiference)
        {
            if (_mouseInput.RightButtonPressed)
                _settings.SelectedPart.Rotation += mouseDiference.Y / 100;
        }
        private void UpdateScaling(Vector2 mouseDiference)
        {
            if (_mouseInput.MiddleButtonPressed)
                _settings.SelectedPart.Scaling += mouseDiference * 0.01f;
        }
        private void UpdateAnimation(GameTime gameTime)
        {
            var keyFrame = _settings.SelectedAnimation.KeyFrames[_currentKeyFrameIndex];

            if (_playing)
            {
                _currentFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 30.0f;

                if (_currentFrameTime > keyFrame.Duration)
                {
                    _currentFrameTime -= keyFrame.Duration;
                    _currentKeyFrameIndex++;
                    if (_currentKeyFrameIndex >= _settings.SelectedAnimation.KeyFrames.Length) _currentKeyFrameIndex = 0;

                    keyFrame = _settings.SelectedAnimation.KeyFrames[_currentKeyFrameIndex];
                }
            }
            else
                _currentKeyFrameIndex = _settings.SelectedKeyFrameIndex;

            if (keyFrame.FrameReference < 0)
                _currentKeyFrameIndex = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _editingArea.Draw(spriteBatch);
            DrawCharacter(spriteBatch);
            DrawAnimation(spriteBatch);
        }
        private void DrawCharacter(SpriteBatch spriteBatch)
        {
            if (_settings.SelectedFrameIndex > 0)
                DrawCharacterFrame(spriteBatch, _characterPosition, 2f, FaceRight, _settings.SelectedFrameIndex - 1, false, 0.2f);

            if (_settings.SelectedFrameIndex < CharacterDefinition.FramesCount - 1)
                DrawCharacterFrame(spriteBatch, _characterPosition, 2f, FaceRight, _settings.SelectedFrameIndex, false, 0.2f);

            DrawCharacterFrame(spriteBatch, _characterPosition, 2f, FaceRight, _settings.SelectedFrameIndex, false, 1);
        }
        private void DrawAnimation(SpriteBatch spriteBatch)
        {
            var fRef = Math.Max(_settings.SelectedAnimation.KeyFrames[_currentKeyFrameIndex].FrameReference, 0);

            DrawCharacterFrame(spriteBatch, _animationPosition, 0.5f, FaceLeft, fRef, true, 1);

            if (_playing)
            {
                if (LegacySuport.DrawClickText(550, 3, "stop"))
                    _playing = false;
            }
            else
            {
                if (LegacySuport.DrawClickText(550, 3, "play"))
                    _playing = true;
            }
        }

        private void DrawCharacterFrame(SpriteBatch spriteBatch, Vector2 loc, float scale, int face, int frameIndex, bool preview, float alpha)
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

                var color = new Color(255, 255, 255, (byte)(alpha * 255));

                if (!preview && _settings.SelectedPartIndex == i && _settings.SelectedFrameIndex == frameIndex)
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