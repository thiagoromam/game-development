namespace MapEditor.Ioc.Api.Settings
{
    public interface ISettings
    {
        string MapPath { get; set; }
        DrawingMode DrawingMode { get; set; }
        int MapLayer { get; set; }
    }
}