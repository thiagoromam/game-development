using System.IO;
using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using MapEditor.MapClasses;
using Microsoft.Xna.Framework;

// ReSharper disable ForCanBeConvertedToForeach

namespace MapEditor.Editor.Buttons.File
{
    public class LoadButton : Button
    {
        private readonly IReadOnlySettings _settings;
        private readonly IMapData _mapData;

        public LoadButton(int x, int y) : base(4, x, y)
        {
            _settings = App.Container.Resolve<IReadOnlySettings>();
            _mapData = App.Container.Resolve<IMapData>();
        }

        public override void Update()
        {
            base.Update();

            if (Clicked)
                Load();
        }

        private void Load()
        {
            var mapPath = string.Format("Content/{0}", _settings.MapPath);
            var file = new BinaryReader(System.IO.File.Open(mapPath, FileMode.Open));

            ReadLedges(file);
            ReadSegments(file);
            ReadGrid(file);

            file.Close();
        }

        private void ReadLedges(BinaryReader file)
        {
            for (var i = 0; i < _mapData.Ledges.Length; i++)
            {
                var ledge = _mapData.Ledges[i];
                ledge.TotalNodes = file.ReadInt32();
                for (var j = 0; j < ledge.TotalNodes; j++)
                    ledge.Nodes[j] = ReadVector2(file);
            }
        }

        private void ReadSegments(BinaryReader file)
        {
            for (var l = 0; l <= _mapData.MaxSegmentsDimension0Index; l++)
            {
                for (var i = 0; i <= _mapData.MaxSegmentsDimension1Index; i++)
                {
                    var index = file.ReadInt32();
                    if (index != -1)
                        _mapData.Segments[l, i] = new MapSegment(index, ReadVector2(file));
                }
            }
        }

        private void ReadGrid(BinaryReader file)
        {
            for (var i = 0; i <= _mapData.MaxGridDimension0Index; i++)
            {
                for (var j = 0; j <= _mapData.MaxGridDimension1Index; j++)
                    _mapData.Grid[i, j] = file.ReadInt32();
            }
        }

        private static Vector2 ReadVector2(BinaryReader file)
        {
            return new Vector2(file.ReadSingle(), file.ReadSingle());
        }
    }
}