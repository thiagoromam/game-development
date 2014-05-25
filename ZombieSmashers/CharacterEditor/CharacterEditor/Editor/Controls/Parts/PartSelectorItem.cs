using System.Globalization;

namespace CharacterEditor.Editor.Controls.Parts
{
    partial class PartSelector
    {
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
                var text = optionValue.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ": ";

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