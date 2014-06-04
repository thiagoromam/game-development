using System;
using System.IO;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers.CharClasses
{
    public class CharDef
    {
        private readonly Animation[] _animations;
        private readonly Frame[] _frames;

        public int HeadIndex;
        public int LegsIndex;
        public string Path;
        public int TorsoIndex;
        public int WeaponIndex;
        public CharacterType CharType = CharacterType.Guy;

        public CharDef(string path)
        {
            _animations = new Animation[64];
            for (var i = 0; i < _animations.Length; i++)
                _animations[i] = new Animation();

            _frames = new Frame[512];
            for (var i = 0; i < _frames.Length; i++)
                _frames[i] = new Frame();

            Path = path;

            Read();
        }

        public Animation[] Animations
        {
            get { return _animations; }
        }

        public Frame[] Frames
        {
            get { return _frames; }
        }

        public void Write()
        {
            var b = new BinaryWriter(File.Open(@"data/" + Path + ".zmx", FileMode.Create));

            b.Write(Path);
            b.Write(HeadIndex);
            b.Write(TorsoIndex);
            b.Write(LegsIndex);
            b.Write(WeaponIndex);

            for (var i = 0; i < _frames.Length; i++)
            {
                b.Write(_frames[i].Name);

                for (var j = 0; j < _frames[i].Parts.Length; j++)
                {
                    var p = _frames[i].Parts[j];
                    b.Write(p.Index);
                    b.Write(p.Location.X);
                    b.Write(p.Location.Y);
                    b.Write(p.Rotation);
                    b.Write(p.Scaling.X);
                    b.Write(p.Scaling.Y);
                    b.Write(p.Flip);
                }
            }

            for (var i = 0; i < _animations.Length; i++)
            {
                b.Write(_animations[i].Name);

                for (var j = 0; j < _animations[i].KeyFrames.Length; j++)
                {
                    var keyframe = _animations[i].KeyFrames[j];
                    b.Write(keyframe.FrameRef);
                    b.Write(keyframe.Duration);
                    var scripts = keyframe.Scripts;
                    for (var s = 0; s < scripts.Length; s++)
                        b.Write(scripts[s]);
                }
            }

            b.Close();
        }

        public void Read()
        {
            var b = new
                BinaryReader(File.Open(@"data/" + Path + ".zmx",
                    FileMode.Open, FileAccess.Read));

            Path = b.ReadString();
            HeadIndex = b.ReadInt32();
            TorsoIndex = b.ReadInt32();
            LegsIndex = b.ReadInt32();
            WeaponIndex = b.ReadInt32();

            for (var i = 0; i < _animations.Length; i++)
            {
                _animations[i].Name = b.ReadString();

                for (var j = 0; j < _animations[i].KeyFrames.Length; j++)
                {
                    var keyframe = _animations[i].KeyFrames[j];
                    keyframe.FrameRef = b.ReadInt32();
                    keyframe.Duration = b.ReadInt32();

                    var scripts = keyframe.Scripts;
                    for (var s = 0; s < scripts.Length; s++)
                        scripts[s] = b.ReadString();
                }
            }

            for (var i = 0; i < _frames.Length; i++)
            {
                _frames[i].Name = b.ReadString();

                for (var j = 0; j < _frames[i].Parts.Length; j++)
                {
                    var p = _frames[i].Parts[j];
                    p.Index = b.ReadInt32();
                    p.Location.X = b.ReadSingle();
                    p.Location.Y = b.ReadSingle();
                    p.Rotation = b.ReadSingle();
                    p.Scaling.X = b.ReadSingle();
                    p.Scaling.Y = b.ReadSingle();
                    p.Flip = b.ReadInt32();
                }
            }

            b.Close();

            Console.WriteLine("Loaded.");
        }
    }
}