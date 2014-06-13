using Microsoft.Xna.Framework.Audio;

namespace ZombieSmashers.Audio
{
    public class Sound
    {
        private static readonly AudioEngine Engine;
        private static readonly SoundBank SoundBank;
        private static readonly WaveBank WaveBank;

        static Sound()
        {
            Engine = new AudioEngine(@"Content/sfx/sfxproj.xgs");
            WaveBank = new WaveBank(Engine, @"Content/sfx/sfxwavs.xwb");
            SoundBank = new SoundBank(Engine, @"Content/sfx/sfxsnds.xsb");
        }

        public static void PlayCue(string cue)
        {
            SoundBank.PlayCue(cue);
        }

        public static Cue GetCue(string cue)
        {
            return SoundBank.GetCue(cue);
        }

        public static void Update()
        {
            Engine.Update();
        }
    }
}