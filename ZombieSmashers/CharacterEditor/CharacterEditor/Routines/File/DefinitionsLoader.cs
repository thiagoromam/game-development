using System;
using System.IO;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Character;
using Funq.Fast;

// ReSharper disable ForCanBeConvertedToForeach
namespace CharacterEditor.Routines.File
{
    public class DefinitionsLoader : IDefinitionsLoader
    {
        private readonly CharacterDefinition _characterDefinition;
        public event Action DefinitionsLoaded;

        public DefinitionsLoader()
        {
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
        }

        public void Load(BinaryReader file)
        {
            _characterDefinition.HeadIndex = file.ReadInt32();
            _characterDefinition.TorsoIndex = file.ReadInt32();
            _characterDefinition.LegsIndex = file.ReadInt32();
            _characterDefinition.WeaponIndex = file.ReadInt32();

            for (var i = 0; i < _characterDefinition.Animations.Length; i++)
            {
                _characterDefinition.Animations[i].Name = file.ReadString();

                for (var j = 0; j < _characterDefinition.Animations[i].KeyFrames.Length; j++)
                {
                    var keyframe = _characterDefinition.Animations[i].KeyFrames[j];
                    keyframe.FrameReference = file.ReadInt32();
                    keyframe.Duration = file.ReadInt32();

                    for (var s = 0; s < keyframe.Scripts.Length; s++)
                        keyframe.Scripts[s] = file.ReadString();
                }
            }

            for (var i = 0; i < _characterDefinition.Frames.Length; i++)
            {
                _characterDefinition.Frames[i].Name = file.ReadString();

                for (var j = 0; j < _characterDefinition.Frames[i].Parts.Length; j++)
                {
                    var p = _characterDefinition.Frames[i].Parts[j];
                    p.Index = file.ReadInt32();
                    p.Location.X = file.ReadSingle();
                    p.Location.Y = file.ReadSingle();
                    p.Rotation = file.ReadSingle();
                    p.Scaling.X = file.ReadSingle();
                    p.Scaling.Y = file.ReadSingle();
                    p.Flip = file.ReadInt32();
                }
            }

            OnDefinitionsLoaded();
        }

        private void OnDefinitionsLoaded()
        {
            var handler = DefinitionsLoaded;
            if (handler != null) handler();
        }
    }
}