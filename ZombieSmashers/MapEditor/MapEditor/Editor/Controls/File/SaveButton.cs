using System.IO;
using Funq.Fast;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;

// ReSharper disable ForCanBeConvertedToForeach
namespace MapEditor.Editor.Controls.File
{
    public class SaveButton : DefaultButton
    {
        private readonly IReadOnlySettings _settings;
        private readonly IReadonlyMapData _mapData;

        public SaveButton(int x, int y) : base(3, x, y)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _mapData = DependencyInjection.Resolve<IReadonlyMapData>();
        }

        public override void Update()
        {
            base.Update();

            if (Clicked)
                Save();
        }

        private void Save()
        {
            var mapPath = string.Format("Content/{0}", _settings.MapPath);
            var file = new BinaryWriter(System.IO.File.Open(mapPath, FileMode.Create));

            WriteLedges(file);
            WriteSegments(file);
            WriteGrid(file);
            
            file.Close();
        }

        private void WriteLedges(BinaryWriter file)
        {
            for (var i = 0; i < _mapData.Ledges.Length; i++)
            {
                var ledge = _mapData.Ledges[i];

                file.Write(ledge.TotalNodes);
                file.Write(ledge.Flags);

                for (var j = 0; j < ledge.TotalNodes; j++)
                {
                    var node = ledge.Nodes[j];

                    file.Write(node.X);
                    file.Write(node.Y);
                }
            }
        }

        private void WriteSegments(BinaryWriter file)
        {
            for (var l = 0; l <= _mapData.MaxSegmentsDimension0Index; l++)
            {
                for (var i = 0; i <= _mapData.MaxSegmentsDimension1Index; i++)
                {
                    var segment = _mapData.Segments[l, i];
                    if (segment == null)
                    {
                        file.Write(-1);
                    }
                    else
                    {
                        file.Write(segment.Index);
                        file.Write(segment.Location.X);
                        file.Write(segment.Location.Y);
                    }
                }
            }
        }

        private void WriteGrid(BinaryWriter file)
        {
            for (var i = 0; i <= _mapData.MaxGridDimension0Index; i++)
            {
                for (var j = 0; j <= _mapData.MaxGridDimension1Index; j++)
                {
                    file.Write(_mapData.Grid[i, j]);
                }
            }
        }
    }
}
