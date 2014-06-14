using System.Globalization;
using Microsoft.Xna.Framework.Audio;

namespace ZombieSmashers.Audio
{
    public class Music
    {
        private static readonly WaveBank Wave;
        private static readonly SoundBank Sound;
        private static Cue _musicCue;
        private static string _musicStr;

        static Music()
        {
            Wave = new WaveBank(Audio.Sound.Engine, @"Content/sfx/musicwavs.xwb", 0, 16);
            Sound = new SoundBank(Audio.Sound.Engine, @"Content/sfx/musicsnds.xsb");
        }

        public static void Play(string musicCueName)
        {
            while(!Wave.IsPrepared)
                Audio.Sound.Update();

            if (string.Compare(_musicStr, musicCueName, true, CultureInfo.InvariantCulture) != 0)
            {
                _musicStr = musicCueName;

                if (_musicCue != null)
                    _musicCue.Dispose();

                _musicCue = Sound.GetCue(_musicStr);
                _musicCue.Play();
            }
        }
    }
}