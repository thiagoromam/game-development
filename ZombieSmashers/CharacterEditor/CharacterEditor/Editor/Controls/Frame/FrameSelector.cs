using System.Globalization;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Frame
{
    public class FrameSelector : TextButtonList<int>
    {
        private readonly IFrameScroll _frameScroll;
        private readonly ISettings _settings;
        private readonly CharacterDefinition _characterDefinition;
        private readonly FrameSelectorItem[] _items;

        public FrameSelector(int x, int y, int yIncrement, IFrameScroll frameScroll)
        {
            _frameScroll = frameScroll;
            _settings = DependencyInjection.Resolve<ISettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _items = new FrameSelectorItem[20];

            for (var i = 0; i < 20; i++)
            {
                var option = AddOption(i, FrameSelectorItem.GetOptionText(i), new Vector2(x, y + i * yIncrement));
                var frame = _characterDefinition.Frames[i];
                _items[i] = new FrameSelectorItem(option, frame);
            }

            _frameScroll.ScrollIndexChanged += UpdateOptions;
        }

        private void UpdateOptions()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                var frameIndex = _frameScroll.ScrollIndex + i;
                _items[i].SetFrame(_characterDefinition.Frames[frameIndex], frameIndex);
            }
        }
        
        private class FrameSelectorItem
        {
            private readonly TextButtonOption _option;
            private Character.Frame _frame;

            public FrameSelectorItem(TextButtonOption option, Character.Frame frame)
            {
                _frame = frame;
                _option = option;
            }

            public void SetFrame(Character.Frame frame, int frameIndex)
            {
                _frame = frame;
                _option.Text = GetOptionText(frameIndex);
            }

            public static string GetOptionText(int frameIndex)
            {
                return frameIndex.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0') + ":";
            }
        }
    }
}