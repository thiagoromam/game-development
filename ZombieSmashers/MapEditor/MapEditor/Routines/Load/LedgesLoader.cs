using System;
using System.IO;
using Funq.Fast;
using MapEditor.Ioc.Api.Map;
using Microsoft.Xna.Framework;

namespace MapEditor.Routines.Load
{
    public class LedgesLoader : ILedgesLoader
    {
        public event Action LedgesLoaded;
        private readonly IMapData _mapData;

        public LedgesLoader()
        {
            _mapData = DependencyInjection.Resolve<IMapData>();
        }

        public void Load(BinaryReader file)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _mapData.Ledges.Length; i++)
            {
                var ledge = _mapData.Ledges[i];
                
                ledge.TotalNodes = file.ReadInt32();
                ledge.Flags = file.ReadInt32();

                for (var j = 0; j < ledge.TotalNodes; j++)
                    ledge.Nodes[j] = new Vector2(file.ReadSingle(), file.ReadSingle());
            }

            if (LedgesLoaded != null)
                LedgesLoaded();
        }
    }
}