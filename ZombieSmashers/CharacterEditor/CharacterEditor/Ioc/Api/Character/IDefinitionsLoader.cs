using System;
using System.IO;

namespace CharacterEditor.Ioc.Api.Character
{
    public interface IDefinitionsLoader
    {
        event Action DefinitionsLoaded;
        void Load(BinaryReader file);
    }
}