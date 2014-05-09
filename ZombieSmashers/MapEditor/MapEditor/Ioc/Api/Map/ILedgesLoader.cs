using System;
using System.IO;

namespace MapEditor.Ioc.Api.Map
{
    public interface ILedgesLoader
    {
        event Action LedgesLoaded;
        void Load(BinaryReader file);
    }
}