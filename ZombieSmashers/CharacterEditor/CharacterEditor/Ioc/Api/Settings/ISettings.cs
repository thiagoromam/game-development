using System;
using CharacterEditor.Character;

namespace CharacterEditor.Ioc.Api.Settings
{
    public delegate void FrameIndexChangedHandler(int previousIndex, int newIndex);
 
    public interface ISettings
    {
        int SelectedFrameIndex { get; set; }
        int SelectedPartIndex { get; set; }

        Frame SelectedFrame { get; }
        Part SelectedPart { get; }

        event FrameIndexChangedHandler SelectedFrameIndexChanged;
        event Action SelectedFrameChanged;
        event Action SelectedPartChanged;
    }
}