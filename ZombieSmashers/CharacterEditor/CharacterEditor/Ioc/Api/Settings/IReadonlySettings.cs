using System;
using CharacterEditor.Character;

namespace CharacterEditor.Ioc.Api.Settings
{
    public interface IReadonlySettings
    {
        int SelectedAnimationIndex { get; }
        int SelectedFrameIndex { get; }
        int SelectedPartIndex { get; }

        Animation SelectedAnimation { get; }
        Frame SelectedFrame { get; }
        Part SelectedPart { get; }

        event AnimationIndexChangedHandler SelectedAnimationIndexChanged;
        event FrameIndexChangedHandler SelectedFrameIndexChanged;
        event Action SelectedAnimationChanged;
        event Action SelectedFrameChanged;
        event Action SelectedPartChanged;
    }
}