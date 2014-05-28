using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using SharedLib;

namespace CharacterEditor.Board
{
    public class CharacterBoard
    {
        private readonly IReadOnlySettings _settings;
        private readonly IMouseInput _mouseInput;
        private readonly AreaRectangle _editingArea;
        private readonly Vector2 _characterPosition;

        public CharacterBoard()
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _editingArea = new AreaRectangle(205, 110, 380, 345, new Color(10, 10, 10, 50));
            _characterPosition = new Vector2(400, 450);
        }

        public void Update()
        {
            if (!CanEdit()) return;
            
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
                _settings.SelectedPart.Rotation += mouseDiference.Y / 100;
        }
        private void UpdateScaling(Vector2 mouseDiference)
        {
            if (_mouseInput.MiddleButtonPressed)
                _settings.SelectedPart.Scaling += mouseDiference * 0.01f;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            _editingArea.Draw(spriteBatch);
            DrawCharacter(spriteBatch);
        }
        private void DrawCharacter(SpriteBatch spriteBatch)
        {
            if (_settings.SelectedFrameIndex > 0)
                BoardExtensions.DrawCharacterFrame(spriteBatch, _characterPosition, 2f, false, _settings.SelectedFrameIndex - 1, false, 0.2f);

            if (_settings.SelectedFrameIndex < CharacterDefinition.FramesCount - 1)
                BoardExtensions.DrawCharacterFrame(spriteBatch, _characterPosition, 2f, false, _settings.SelectedFrameIndex, false, 0.2f);

            BoardExtensions.DrawCharacterFrame(spriteBatch, _characterPosition, 2f, false, _settings.SelectedFrameIndex, false, 1);
        }
        
        private bool CanEdit()
        {
            return _editingArea.Area.Contains(_mouseInput.Position);
        }
    }
}