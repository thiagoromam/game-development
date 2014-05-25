using System.Globalization;
using System.Linq;
using System.Text;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Helpers;
using Microsoft.Xna.Framework;

namespace CharacterEditor.Editor.Controls.Frames
{
    public interface IFrameSelector
    {
        TextButtonList<int>.TextButtonOption SelectedOption { get; }
    }

    public class FrameSelector : TextButtonList<int>, IFrameSelector
    {
        private readonly IFramesScroll _framesScroll;
        private readonly ISettings _settings;
        private readonly CharacterDefinition _characterDefinition;
        private readonly TextButtonOption[] _options;

        public FrameSelector(int x, int y, int yIncrement, IFramesScroll framesScroll)
        {
            _framesScroll = framesScroll;
            _settings = DependencyInjection.Resolve<ISettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _options = new TextButtonOption[_framesScroll.Limit];

            for (var i = 0; i < _options.Length; i++)
                _options[i] = AddOption(i, GetFrameText(i), new Vector2(x, y + i * yIncrement));

            SelectedValue = _settings.SelectedFrameIndex;
            Change = ValueChange;
            _framesScroll.ScrollIndexChanged += UpdateOptions;
            _settings.SelectedFrameIndexChanged += UpdateFrameWithoutName;
        }

        private void UpdateFrameWithoutName(int previousIndex, int newIndex)
        {
            var previousFrame = _characterDefinition.Frames[previousIndex];
            if (previousFrame.Name.HasValue()) return;

            previousFrame.Name = "frame" + previousIndex;

            if (_framesScroll.IsIndexVisible(previousIndex))
                _options.Single(o => o.Value == previousIndex).Text = GetFrameText(previousIndex);
        }

        private void ValueChange(int? previousValue, int? newValue)
        {
            if (newValue.HasValue)
            {
                _options.Single(o => o.Value == newValue.Value).Text = GetFrameText(newValue.Value);

                var previousFrameIndex = _settings.SelectedFrameIndex;
                _settings.SelectedFrameIndex = newValue.Value;
                CopyFrame(previousFrameIndex, newValue.Value);
            }

            if (previousValue.HasValue)
                _options.Single(o => o.Value == previousValue.Value).Text = GetFrameText(previousValue.Value);
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
            var isCurrentFrameVisible = _framesScroll.IsCurrentFrameVisible();
            if (!isCurrentFrameVisible)
                ClearValue();

            for (var i = 0; i < _options.Length; i++)
            {
                var option = _options[i];
                option.Value = _framesScroll.ScrollIndex + i;
                option.Text = GetFrameText(option.Value);
            }

            if (isCurrentFrameVisible)
                SelectedValue = _settings.SelectedFrameIndex;
        }

        private string GetFrameText(int frameIndex)
        {
            var text = new StringBuilder();
            text.Append(frameIndex.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
            text.Append(":");

            if (frameIndex != _settings.SelectedFrameIndex)
            {
                var frame = _characterDefinition.Frames[frameIndex];
                text.AppendFormat(" {0}", frame.Name);
            }

            return text.ToString();
        }
    }
}