using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    public partial class KeyFrameSelector : TextButtonList<int>
    {
        private readonly IKeyFramesScroll _framesScroll;
        private readonly ISettings _settings;
        private readonly KeyFrameSelectorItem[] _items;

        public KeyFrameSelector(int x, int y, int yIncrement, IKeyFramesScroll framesScroll)
        {
            _framesScroll = framesScroll;
            _settings = DependencyInjection.Resolve<ISettings>();
            _items = new KeyFrameSelectorItem[_framesScroll.Limit];

            for (var i = 0; i < _items.Length; i++)
            {
                var option = AddOption(i, null, new Vector2(x, y + i * yIncrement));
                _items[i] = new KeyFrameSelectorItem(option, i);
            }

            SelectedValue = _settings.SelectedKeyFrameIndex;
            Change = ValueChange;
            _settings.SelectedKeyFrameChanged += UpdateItems;
            _settings.SelectedAnimation.KeyFramesChanged += UpdateItems;
            _framesScroll.ScrollIndexChanged += UpdateItems;
        }

        private void ValueChange(int? previousValue, int? newValue)
        {
            if (newValue.HasValue)
                _settings.SelectedKeyFrameIndex = newValue.Value;
        }

        private void UpdateItems()
        {
            var isCurrentKeyFrameVisible = _framesScroll.IsCurrentKeyFrameVisible();
            if (!isCurrentKeyFrameVisible)
                ClearValue();

            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                item.Option.Value = _framesScroll.ScrollIndex + i;
                item.SetKeyFrame(item.Option.Value);
            }

            if (isCurrentKeyFrameVisible)
                SelectedValue = _settings.SelectedKeyFrameIndex;
        }
    }
}
