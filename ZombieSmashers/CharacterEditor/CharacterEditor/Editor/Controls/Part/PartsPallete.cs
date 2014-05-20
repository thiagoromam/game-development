using CharacterEditor.Ioc.Api.Editor;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Editor.Controls.Part
{
    public class PartsPallete : IPartsPalleteComponent
    {
        private const int YIncrement = 15;
        private const int X = 600;
        private const int Y = 5;
        private readonly IReadonlySettings _settings;
        private readonly PartSelector _partSelector;
        private readonly SwapUpButton _swapUpButton;
        private readonly SwapDownButton _swapDownButton;
        private readonly MirrorButton _mirrorButton;
        private readonly ResetButton _resetButton;
        private readonly DeleteButton _deleteButton;

        public PartsPallete()
        {
            _settings = DependencyInjection.Resolve<IReadonlySettings>();
            
            _partSelector = new PartSelector(X, Y, YIncrement);
            _swapUpButton = new SwapUpButton(X + 100, Y);
            _swapDownButton = new SwapDownButton(X + 120, Y);
            _mirrorButton = new MirrorButton(X + 140, Y);
            _resetButton = new ResetButton(X + 162, Y);
            _deleteButton = new DeleteButton(X + 180, Y);

            _settings.SelectedPartChanged += UpdateSelectedPartOptions;
            UpdateSelectedPartOptions();
        }

        private void UpdateSelectedPartOptions()
        {
            _mirrorButton.UpdateControl(_settings.SelectedPart);
            _resetButton.UpdateControl(_settings.SelectedPart);
            _deleteButton.UpdateControl(_settings.SelectedPart);

            var y = Y + _settings.SelectedPartIndex * YIncrement;
            _swapUpButton.Position = new Vector2(_swapUpButton.Position.X, y);
            _swapDownButton.Position = new Vector2(_swapDownButton.Position.X, y);
            _mirrorButton.Position.Y = y;
            _resetButton.Position.Y = y;
            _deleteButton.Position.Y = y;
        }

        public void Update()
        {
            _partSelector.Update();
            _swapUpButton.Update();
            _swapDownButton.Update();
            _mirrorButton.Update();
            _swapDownButton.Update();
            _mirrorButton.Update();
            _resetButton.Update();
            _deleteButton.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _partSelector.Draw();
            _swapUpButton.Draw(spriteBatch);
            _swapDownButton.Draw(spriteBatch);
            _mirrorButton.Draw();
            _resetButton.Draw();
            _deleteButton.Draw();
        }
    }
}