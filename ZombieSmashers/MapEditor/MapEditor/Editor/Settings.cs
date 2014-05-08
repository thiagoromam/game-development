using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor
{
    public class Settings : ISettings, IReadableSettings
    {
        public Settings()
        {
            MapPath = "maps.zdx";
            DrawingMode = DrawingMode.SegmentSelection;
            MapLayer = 1;
        }

        public string MapPath { get; set; }
        public DrawingMode DrawingMode { get; set; }
        public int MapLayer { get; set; }
    }
}