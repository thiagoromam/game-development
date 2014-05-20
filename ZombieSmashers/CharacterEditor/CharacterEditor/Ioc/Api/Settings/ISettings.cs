using System;
using CharacterEditor.Character;

namespace CharacterEditor.Ioc.Api.Settings
{
    public interface ISettings
    {
        int SelectedFrameIndex { get; set; }
        int SelectedPartIndex { get; set; }

        Frame SelectedFrame { get; }
        Part SelectedPart { get; }

        event Action SelectedFrameChanged;
        event Action SelectedPartChanged;
    }
}