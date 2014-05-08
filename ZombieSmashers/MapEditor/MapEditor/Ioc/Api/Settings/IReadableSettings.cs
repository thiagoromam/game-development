namespace MapEditor.Ioc.Api.Settings
{
    public interface IReadableSettings
    {
        string MapPath { get; }
        DrawingMode DrawingMode { get; }
        int MapLayer { get; }
    }
}