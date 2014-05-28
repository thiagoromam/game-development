using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Board
{
    public static class BoardExtensions
    {
        private static IReadOnlySettings _settings;
        private static CharacterDefinition _characterDefinition;

        public static void Initialize()
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
        }

        public static void DrawCharacterFrame(SpriteBatch spriteBatch, Vector2 loc, float scale, bool faceLeft, int frameIndex, bool preview, float alpha)
        {
            var source = new Rectangle();

            var frame = _characterDefinition.Frames[frameIndex];

            spriteBatch.Begin();

            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index == -1) continue;

                source.X = ((part.Index % 64) % 5) * 64;
                source.Y = ((part.Index % 64) / 5) * 64;
                source.Width = 64;
                source.Height = 64;

                if (part.Index >= 192)
                {
                    source.X = ((part.Index % 64) % 3) * 80;
                    source.Width = 80;
                }

                var location = part.Location * scale + loc;
                var scaling = part.Scaling * scale;
                if (part.Index >= 128)
                    scaling *= 1.35f;

                var rotation = part.Rotation;
                if (faceLeft)
                {
                    rotation = -rotation;
                    location.X -= part.Location.X * scale * 2;
                }

                Texture2D texture = null;

                switch (part.Index / 64)
                {
                    case 0: texture = Art.Heads[_characterDefinition.HeadIndex]; break;
                    case 1: texture = Art.Torsos[_characterDefinition.TorsoIndex]; break;
                    case 2: texture = Art.Legs[_characterDefinition.LegsIndex]; break;
                    case 3: texture = Art.Weapons[_characterDefinition.WeaponIndex]; break;
                }

                var color = new Color(255, 255, 255, (byte)(alpha * 255));

                if (!preview && _settings.SelectedPartIndex == i && _settings.SelectedFrameIndex == frameIndex)
                {
                    color.G = 0;
                    color.B = 0;
                }

                var flip = faceLeft && part.Flip == 1 || !faceLeft && part.Flip == 0;

                if (texture == null) continue;
                var origin = new Vector2(source.Width / 2f, 32f);

                spriteBatch.Draw(
                    texture,
                    location,
                    source,
                    color,
                    rotation,
                    origin,
                    scaling,
                    flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    1
                );
            }

            spriteBatch.End();
        }
    }
}