using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace NeonVectorShooter
{
    public static class Sound
    {
        private static readonly Random Random;
        private static List<SoundEffect> _explosions;
        private static List<SoundEffect> _shoots;
        private static List<SoundEffect> _spawns;

        static Sound()
        {
            Random = new Random();
        }

        public static Song Music { get; private set; }
        public static SoundEffect Explosion
        {
            get { return _explosions.GetRandomEffect(); }
        }
        public static SoundEffect Shoot
        {
            get { return _shoots.GetRandomEffect(); }
        }
        public static SoundEffect Spawn
        {
            get { return _spawns.GetRandomEffect(); }
        }

        public static void Load(ContentManager content)
        {
            Music = content.Load<Song>("Sounds/Musics/Music");

            _explosions = LoadSoundEffects(content, 8, "Sounds/Effects/Explosions/explosion-0");
            _shoots = LoadSoundEffects(content, 4, "Sounds/Effects/Shoots/shoot-0");
            _spawns = LoadSoundEffects(content, 8, "Sounds/Effects/Spawns/spawn-0");
        }

        private static List<SoundEffect> LoadSoundEffects(ContentManager content, int count, string baseName)
        {
            return Enumerable.Range(1, count).Select(i => content.Load<SoundEffect>(baseName + i)).ToList();
        }

        private static SoundEffect GetRandomEffect(this IList<SoundEffect> sounds)
        {
            return sounds[Random.Next(sounds.Count)];
        }
    }
}