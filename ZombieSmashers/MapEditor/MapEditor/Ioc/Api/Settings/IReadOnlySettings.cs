namespace MapEditor.Ioc.Api.Settings
{
    public interface IReadOnlySettings
    {
        string MapPath { get; }
        DrawingMode CurrentDrawingMode { get; }
        int CurrentMapLayer { get; }
        int SelectedLedge { get; }
    }
}