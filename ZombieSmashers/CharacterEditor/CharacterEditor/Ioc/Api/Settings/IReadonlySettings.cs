using System;
using CharacterEditor.Character;

namespace CharacterEditor.Ioc.Api.Settings
{
    public interface IReadOnlySettings
    {
        int SelectedAnimationIndex { get; }
        int SelectedKeyFrameIndex { get; }
        int SelectedFrameIndex { get; }
        int SelectedPartIndex { get; }

        Animation SelectedAnimation { get; }
        KeyFrame SelectedKeyFrame { get; }
        Frame SelectedFrame { get; }
        Part SelectedPart { get; }

        event AnimationIndexChangedHandler SelectedAnimationIndexChanged;
        event FrameIndexChangedHandler SelectedFrameIndexChanged;
        event Action SelectedAnimationChanged;
        event Action SelectedKeyFrameChanged;
        event Action SelectedFrameChanged;
        event Action SelectedPartChanged;
    }
}