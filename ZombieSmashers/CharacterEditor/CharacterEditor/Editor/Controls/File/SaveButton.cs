using System.IO;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;

// ReSharper disable ForCanBeConvertedToForeach
namespace CharacterEditor.Editor.Controls.File
{
    public class SaveButton : SharedLib.Gui.Controls.File.SaveButton
    {
        private readonly IReadOnlySettings _settings;
        private readonly CharacterDefinition _characterDefinition;

        public SaveButton()
            : base(200, 5)
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
        }

        protected override void Save()
        {
            var b = new BinaryWriter(System.IO.File.Open("Content/" + _settings.FileName + ".zmx", FileMode.Create));

            b.Write(_characterDefinition.HeadIndex);
            b.Write(_characterDefinition.TorsoIndex);
            b.Write(_characterDefinition.LegsIndex);
            b.Write(_characterDefinition.WeaponIndex);

            for (var i = 0; i < _characterDefinition.Animations.Length; i++)
            {
                b.Write(_characterDefinition.Animations[i].Name);

                for (var j = 0; j < _characterDefinition.Animations[i].KeyFrames.Length; j++)
                {
                    var keyframe = _characterDefinition.Animations[i].KeyFrames[j];
                    b.Write(keyframe.FrameReference);
                    b.Write(keyframe.Duration);
                    for (var s = 0; s < keyframe.Scripts.Length; s++)
                        b.Write(keyframe.Scripts[s]);
                }
            }

            for (var i = 0; i < _characterDefinition.Frames.Length; i++)
            {
                b.Write(_characterDefinition.Frames[i].Name);

                for (var j = 0; j < _characterDefinition.Frames[i].Parts.Length; j++)
                {
                    var p = _characterDefinition.Frames[i].Parts[j];
                    b.Write(p.Index);
                    b.Write(p.Location.X);
                    b.Write(p.Location.Y);
                    b.Write(p.Rotation);
                    b.Write(p.Scaling.X);
                    b.Write(p.Scaling.Y);
                    b.Write(p.Flip);
                }
            }

            b.Close();
        }
    }
}