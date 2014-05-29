using System.IO;
using Funq.Fast;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using MapEditor.MapClasses;
using Microsoft.Xna.Framework;

// ReSharper disable ForCanBeConvertedToForeach

namespace MapEditor.Editor.Controls.File
{
    public class LoadButton : SharedLib.Gui.Controls.File.LoadButton
    {
        private readonly IReadOnlySettings _settings;
        private readonly IMapData _mapData;
        private readonly ILedgesLoader _ledgesLoader;

        public LoadButton(int x, int y) : base(x, y)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _mapData = DependencyInjection.Resolve<IMapData>();
            _ledgesLoader = DependencyInjection.Resolve<ILedgesLoader>();
        }

        protected override void Load()
        {
            var mapPath = string.Format("Content/{0}", _settings.MapPath);
            var file = new BinaryReader(System.IO.File.Open(mapPath, FileMode.Open));

            _ledgesLoader.Load(file);
            ReadSegments(file);
            ReadGrid(file);

            file.Close();
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