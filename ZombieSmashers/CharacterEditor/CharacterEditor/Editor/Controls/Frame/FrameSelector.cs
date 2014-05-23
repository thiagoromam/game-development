using System.Globalization;
using System.Text;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Frame
{
    public class FrameSelector : TextButtonList<int>
    {
        private const string FrameNameEditorDefaultText = "<name>";
        private readonly IFrameScroll _frameScroll;
        private readonly ISettings _settings;
        private readonly CharacterDefinition _characterDefinition;
        private readonly FrameSelectorItem[] _items;
        private readonly TextEditor _frameNameEditor;

        public FrameSelector(int x, int y, int yIncrement, IFrameScroll frameScroll)
        {
            _frameScroll = frameScroll;
            _settings = DependencyInjection.Resolve<ISettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _frameNameEditor = new TextEditor(x + 41, y) { Text = FrameNameEditorDefaultText };
            _items = new FrameSelectorItem[20];

            for (var i = 0; i < 20; i++)
            {
                var option = AddOption(i, GetFrameText(i), new Vector2(x, y + i * yIncrement));
                var frame = _characterDefinition.Frames[i];
                _items[i] = new FrameSelectorItem(option, i);

                if (i == _settings.SelectedFrameIndex)
                {
                    _frameNameEditor.Text = frame.Name != string.Empty ? frame.Name : FrameNameEditorDefaultText;
                    _frameNameEditor.Position.Y = option.Position.Y;
                }
            }

            Value = _settings.SelectedFrameIndex;
            Change = ValueChange;
            _frameNameEditor.Change = v => _characterDefinition.Frames[_items[Value].FrameIndex].Name = v;
            _frameScroll.ScrollIndexChanged += UpdateOptions;
        }

        private void ValueChange(int previousValue, int newValue)
        {
            //_settings.SelectedFrameIndex = newValue;

            var previousSelectedItem = _items[previousValue];
            previousSelectedItem.Option.Text = GetFrameText(previousSelectedItem.FrameIndex);

            var newSelectedItem = _items[newValue];
            newSelectedItem.Option.Text = GetFrameText(newSelectedItem.FrameIndex);
            
            _frameNameEditor.Text = _characterDefinition.Frames[newSelectedItem.FrameIndex].Name;
            if (_frameNameEditor.Text == string.Empty)
                _frameNameEditor.Text = FrameNameEditorDefaultText;

            _frameNameEditor.Position.Y = newSelectedItem.Option.Position.Y;
        }

        private void UpdateOptions()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                
                item.FrameIndex = _frameScroll.ScrollIndex + i; 
                item.Option.Text = GetFrameText(item.FrameIndex);
            }
        }

        public override void Update()
        {
            base.Update();
            _frameNameEditor.Update();
        }

        public override void Draw()
        {
            base.Draw();
            _frameNameEditor.Draw();
        }

        private string GetFrameText(int frameIndex)
        {
            var text = new StringBuilder();
            text.Append(frameIndex.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
            text.Append(":");

            if (frameIndex != Value)
            {
                var frame = _characterDefinition.Frames[frameIndex];
                text.AppendFormat(" {0}", frame.Name);
            }

            return text.ToString();
        }

        private class FrameSelectorItem
        {
            public readonly TextButtonOption Option;
            public int FrameIndex;

            public FrameSelectorItem(TextButtonOption option, int frameIndex)
            {
                FrameIndex = frameIndex;
                Option = option;
            }
        }
    }
}