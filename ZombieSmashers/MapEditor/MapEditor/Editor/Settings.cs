using MapEditor.Ioc.Api.Settings;

namespace MapEditor.Editor
{
    public class Settings : ISettings, IReadOnlySettings
    {
        public Settings()
        {
            MapPath = "maps.zmx";
            CurrentDrawingMode = DrawingMode.SegmentSelection;
            CurrentMapLayer = 1;
        }

        public string MapPath { get; set; }
        public DrawingMode CurrentDrawingMode { get; set; }
        public int CurrentMapLayer { get; set; }
        public int SelectedLedge { get; set; }
    }
}