using System.Globalization;
using System.Reflection;
using System.Text;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;

namespace CharacterEditor.Editor.Controls.KeyFrames
{
    partial class KeyFrameSelector
    {
        private class KeyFrameSelectorItem
        {
            private readonly IReadOnlySettings _settings;
            private readonly CharacterDefinition _characterDefinition;
            public readonly TextButtonOption Option;
            private KeyFrame _keyFrame;
            private Frame _frame;

            public KeyFrameSelectorItem(TextButtonOption option, int keyFrameIndex)
            {
                _settings = DependencyInjection.Resolve<IReadOnlySettings>();
                _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
                Option = option;
                SetKeyFrame(keyFrameIndex);
            }

            public void SetKeyFrame(int keyFrameIndex)
            {
                if (_keyFrame != null)
                    _keyFrame.FrameReferenceChanged -= Bind;

                _keyFrame = _settings.SelectedAnimation.KeyFrames[keyFrameIndex];
                _keyFrame.FrameReferenceChanged += Bind;

                Bind();
            }

            private void Bind()
            {
                if (_frame != null)
                    _frame.NameChanged -= UpdateText;

                _frame = _keyFrame.FrameReference > -1 ? _characterDefinition.Frames[_keyFrame.FrameReference] : null;

                if (_frame != null)
                    _frame.NameChanged += UpdateText;

                UpdateText();
            }

            private void UpdateText()
            {
                var name = new StringBuilder();

                name.Append(Option.Value.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
                name.Append(":");

                if (_frame != null)
                    name.AppendFormat(" {0}", _frame.Name);

                Option.Text = name.ToString();
            }
        }
    }
}