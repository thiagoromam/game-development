namespace MapEditor.Ioc.Api.Settings
{
    public interface IReadOnlySettings
    {
        string MapPath { get; }
        DrawingMode DrawingMode { get; }
        int MapLayer { get; }
    }
}