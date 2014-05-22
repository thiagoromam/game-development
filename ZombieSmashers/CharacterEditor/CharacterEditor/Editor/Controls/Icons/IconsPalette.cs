using System.Collections.Generic;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MouseLib.Api;
using SharedLib;

// ReSharper disable ForCanBeConvertedToForeach
namespace CharacterEditor.Editor.Controls.Icons
{
    public partial class IconsPalette : IControlComponent, IControl
    {
        private readonly CharacterDefinition _characterDefinition;
        private readonly List<IconPaletteItem> _items;
        private readonly IMouseInput _mouseInput;
        private readonly IReadonlySettings _settings;
        
        public IconsPalette()
        {
            _items = new List<IconPaletteItem>();
            _characterDefinition = DependencyInjection.Resolve<CharacterDefinition>();
            _mouseInput = DependencyInjection.Resolve<IMouseInput>();
            _settings = DependencyInjection.Resolve<IReadonlySettings>();

            for (var l = 0; l < 4; l++)
            {
                for (var i = 0; i < 25; i++)
                {
                    var source = new Rectangle((i % 5) * 64, (i / 5) * 64, 64, 64);
                    var destination = new Rectangle(i * 23, 467 + l * 32, 23, 32);

                    if (l == 3)
                    {
                        source.X = (i % 4) * 80;
                        source.Y = (i / 4) * 64;
                        source.Width = 80;

                        if (i < 15)
                        {
                            destination.X = i * 30;
                            destination.Width = 30;
                        }
                    }

                    _items.Add(new IconPaletteItem(i + 64 * l, l, source, destination));
                }
            }
        }

        public void Update()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                if (_mouseInput.LeftButtonClick && item.Destination.Contains(_mouseInput.Position))
                    _settings.SelectedPart.Index = item.Index;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                spriteBatch.Draw(SharedArt.Null, item.Destination, new Color(0, 0, 0, 25));

                Texture2D texture = null;
                switch (item.TextureIndex)
                {
                    case 0:
                        texture = Art.Heads[_characterDefinition.HeadIndex];
                        break;
                    case 1:
                        texture = Art.Torsos[_characterDefinition.TorsoIndex];
                        break;
                    case 2:
                        texture = Art.Legs[_characterDefinition.LegsIndex];
                        break;
                    case 3:
                        texture = Art.Weapons[_characterDefinition.WeaponIndex];
                        break;
                }

                if (texture == null) continue;
                
                spriteBatch.Draw(texture, item.Destination, item.Source, Color.White);
            }

            spriteBatch.End();
        }
    }
}