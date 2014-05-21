using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Part
{
    public class PartSelector : TextButtonList<int>
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
            Change += v => _settings.SelectedPartIndex = v;
            _settings.SelectedFrame.PartsChanged += UpdateOptions;
        }

        private void UpdateOptions()
        {
            for (var i = 0; i < _items.Length; i++)
                _items[i].Part = _settings.SelectedFrame.Parts[i];
            
            Value = _settings.SelectedPartIndex;
        }

        private class PartSelectorItem
        {
            private readonly TextButtonOption _option;
            private Character.Part _part;

            public PartSelectorItem(TextButtonOption option, Character.Part part)
            {
                _option = option;
                Part = part;
            }

            public Character.Part Part
            {
                set
                {
                    var partChanged = _part != null;
                    if (partChanged)
                        _part.IndexChanged -= UpdateText;

                    _part = value;
                    _part.IndexChanged += UpdateText;

                    if (partChanged)
                        UpdateText();
                }
            }

            private void UpdateText()
            {
                _option.Text = GetOptionText(_part, _option.Value);
            }

            public static string GetOptionText(Character.Part part, int optionValue)
            {
                var text = optionValue + ": ";

                if (part.Index >= 192)
                    text += "weapon" + part.Index;
                else if (part.Index >= 128)
                    text += "legs" + part.Index;
                else if (part.Index >= 74)
                    text += "arms" + part.Index;
                else if (part.Index >= 64)
                    text += "torso" + part.Index;
                else if (part.Index >= 0)
                    text += "head" + part.Index;

                return text;
            }
        }
    }
}
