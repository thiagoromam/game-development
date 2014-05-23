using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Part
{
    public partial class PartSelector : TextButtonList<int>
    {
        private readonly PartSelectorItem[] _items;
        private readonly ISettings _settings;

        public PartSelector(int x, int y, int yIncrement)
        {
            _settings = DependencyInjection.Resolve<ISettings>();
            _items = new PartSelectorItem[_settings.SelectedFrame.Parts.Length];

            for (var i = 0; i < _settings.SelectedFrame.Parts.Length; i++)
            {
                var part = _settings.SelectedFrame.Parts[i];
                var option = AddOption(i, PartSelectorItem.GetOptionText(part, i), new Vector2(x, y + i * yIncrement));

                _items[i] = new PartSelectorItem(option, part);
            }

            Value = _settings.SelectedPartIndex;
            Change = (p, v) => _settings.SelectedPartIndex = v;
            _settings.SelectedFrame.PartsChanged += UpdateOptions;
        }

        private void UpdateOptions()
        {
            for (var i = 0; i < _items.Length; i++)
                _items[i].Part = _settings.SelectedFrame.Parts[i];
            
            Value = _settings.SelectedPartIndex;
        }
    }
}
