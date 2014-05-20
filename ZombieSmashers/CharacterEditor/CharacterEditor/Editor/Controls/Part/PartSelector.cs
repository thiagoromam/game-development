using System;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Part
{
    public class PartSelector : TextButtonList<int>
    {
        private readonly TextButtonOption[] _options;
        private readonly ISettings _settings;

        public PartSelector(int x, int y, int yIncrement)
        {
            _settings = DependencyInjection.Resolve<ISettings>();
            _options = new TextButtonOption[_settings.SelectedFrame.Parts.Length];

            for (var i = 0; i < _settings.SelectedFrame.Parts.Length; i++)
            {
                var part = _settings.SelectedFrame.Parts[i];
                var option = AddOption(i, GetStringValue(part, i), new Vector2(x, y + i * yIncrement));
                var iCopy = i;
                part.IndexChanged += () => option.Text = GetStringValue(part, iCopy);
                _options[i] = option;
            }

            Value = _settings.SelectedPartIndex;
            Change += v => _settings.SelectedPartIndex = v;

            _settings.SelectedFrame.PartsChanged += UpdateOptions;
        }

        private void UpdateOptions()
        {
            for (var i = 0; i < _options.Length; i++)
            {
                var option = _options[i];
                var part = _settings.SelectedFrame.Parts[i];
                option.Text = GetStringValue(part, i);
            }
        }

        private static string GetStringValue(Character.Part part, int i)
        {
            var text = i + ": ";

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
