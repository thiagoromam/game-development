using MapEditor.MapClasses;

namespace MapEditor.Ioc.Api.Map
{
    public interface IMapData
    {
        int MaxGridDimension0Index { get; }
        int MaxGridDimension1Index { get; }
        int MaxSegmentsDimension0Index { get; }
        int MaxSegmentsDimension1Index { get; }
        SegmentDefinition[] Definitions { get; set; }
        MapSegment[,] Segments { get; set; }
        int[,] Grid { get; set; }
        Ledge[] Ledges { get; set; }

        int AddSegment(int layer, int index);
    }
}