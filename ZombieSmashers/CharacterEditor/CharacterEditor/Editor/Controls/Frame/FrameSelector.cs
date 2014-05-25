using System.Globalization;
using System.Linq;
using System.Text;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Frame
{
    public partial class FrameSelector : TextButtonList<int>
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
                var option = AddOption(i, GetFrameText(i), new Vector2(x, y + i * yIncrement));
                _items[i] = new FrameSelectorItem(option, i);
            }

            SelectedValue = _settings.SelectedFrameIndex;
            Change = ValueChange;
            _frameScroll.ScrollIndexChanged += UpdateOptions;
            _settings.SelectedFrameIndexChanged += UpdateFrameWithoutName;
        }

        private void UpdateFrameWithoutName(int previousIndex, int newIndex)
        {
            var previousFrame = _characterDefinition.Frames[previousIndex];
            if (previousFrame.Name.HasValue()) return;
            
            previousFrame.Name = "frame" + previousIndex;
            _items.Single(i => i.FrameIndex == previousIndex).Option.Text = GetFrameText(previousIndex);
        }

        private void ValueChange(int? previousValue, int? newValue)
        {
            if (previousValue.HasValue)
            {
                var previousSelectedItem = _items[previousValue.Value];
                previousSelectedItem.Option.Text = GetFrameText(previousSelectedItem.FrameIndex);
            }

            if (newValue.HasValue)
            {
                var newSelectedItem = _items[newValue.Value];
                newSelectedItem.Option.Text = GetFrameText(newSelectedItem.FrameIndex);

                var previousFrameIndex = _settings.SelectedFrameIndex;
                _settings.SelectedFrameIndex = newValue.Value;
                CopyFrame(previousFrameIndex, newValue.Value);
            }
        }

        private void CopyFrame(int sourceIndex, int destinationIndex)
        {
            if (_characterDefinition.Frames[destinationIndex].Name.HasValue())
                return;

            var sourceFrame = _characterDefinition.Frames[sourceIndex];
            var destinationFrame = _characterDefinition.Frames[destinationIndex];

            for (var i = 0; i < sourceFrame.Parts.Length; i++)
            {
                var sourcePart = sourceFrame.Parts[i];
                var destinationPart = destinationFrame.Parts[i];

                destinationPart.Index = sourcePart.Index;
                destinationPart.Location = sourcePart.Location;
                destinationPart.Rotation = sourcePart.Rotation;
                destinationPart.Scaling = sourcePart.Scaling;
            }
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

        private string GetFrameText(int frameIndex)
        {
            var text = new StringBuilder();
            text.Append(frameIndex.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
            text.Append(":");

            if (frameIndex != SelectedValue)
            {
                var frame = _characterDefinition.Frames[frameIndex];
                text.AppendFormat(" {0}", frame.Name);
            }

            return text.ToString();
        }
    }
}