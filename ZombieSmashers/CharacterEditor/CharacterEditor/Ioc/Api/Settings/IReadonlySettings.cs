using System;
using CharacterEditor.Character;

namespace CharacterEditor.Ioc.Api.Settings
{
    public interface IReadonlySettings
    {
        int SelectedFrameIndex { get; }
        int SelectedPartIndex { get; }

        Frame SelectedFrame { get; }
        Part SelectedPart { get; }

        event FrameIndexChangedHandler SelectedFrameIndexChanged;
        event Action SelectedFrameChanged;
        event Action SelectedPartChanged;
    }
}