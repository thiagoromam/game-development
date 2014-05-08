using MapEditor.MapClasses;

namespace MapEditor.Ioc.Api.Map
{
    public interface IReadonlyMapData
    {
        int MaxGridDimension0Index { get; }
        int MaxGridDimension1Index { get; }
        int MaxSegmentsDimension0Index { get; }
        int MaxSegmentsDimension1Index { get; }
        SegmentDefinition[] Definitions { get; }
        MapSegment[,] Segments { get; }
        int[,] Grid { get; }
        Ledge[] Ledges { get; }
    }
}