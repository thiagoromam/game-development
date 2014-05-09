namespace MapEditor.Ioc.Api.Settings
{
    public interface ISettings
    {
        string MapPath { get; set; }
        DrawingMode CurrentDrawingMode { get; set; }
        int CurrentMapLayer { get; set; }
        int SelectedLedge { get; set; }
    }
}