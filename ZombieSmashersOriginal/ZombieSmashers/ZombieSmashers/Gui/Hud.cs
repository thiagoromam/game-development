using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.MapClasses;

namespace ZombieSmashers.Gui
{
    public class Hud
    {
        private readonly SpriteBatch _sprite;
        private readonly Texture2D _spritesTex;
        private readonly Texture2D _nullTex;
        private readonly Character[] _character;
        private readonly Map _map;

        private readonly ScoreDraw _scoreDraw;

        private float _heartFrame;
        private float _fHp;

        public Hud(SpriteBatch sprite, Texture2D spritesTex, Texture2D nullTex, Character[] character, Map map)
        {
            _sprite = sprite;
            _spritesTex = spritesTex;
            _character = character;
            _map = map;
            _nullTex = nullTex;
            _scoreDraw = new ScoreDraw(_sprite, _spritesTex);
        }

        public void Update()
        {
            _heartFrame += Game1.FrameTime;
            if (_heartFrame > 6.28f)
                _heartFrame -= 6.28f;

            if (_character[0].Hp > _fHp)
            {
                _fHp += Game1.FrameTime * 15f;
                if (_fHp > _character[0].Hp)
                    _fHp = _character[0].Hp;
            }

            if (_character[0].Hp < _fHp)
            {
                _fHp -= Game1.FrameTime * 15f;
                if (_fHp < _character[0].Hp)
                    _fHp = _character[0].Hp;
            }
        }

        public void Draw()
        {
            _sprite.Begin();

            _scoreDraw.Draw(Game1.Score, new Vector2(50f, 78f), Color.White, Justification.Left);

            var fProg = _fHp / _character[0].Mhp;
            // ReSharper disable once PossibleLossOfFraction
            float prog = _character[0].Hp / _character[0].Mhp;
            fProg *= 5f;
            prog *= 5f;

            for (var i = 0; i < 5; i++)
            {
                var r = (float)Math.Cos(_heartFrame * 2.0 + i) * 0.1f;

                _sprite.Draw(_spritesTex, new Vector2(66f + i * 32f, 66f), new Rectangle(i * 32, 192, 32, 32),
                    new Color(new Vector4(0.5f, 0f, 0f, .25f)), r, new Vector2(16f, 16f), 1.25f, SpriteEffects.None, 1f);

                var ta = fProg - i;
                if (ta > 1f)
                    ta = 1f;

                if (ta > 0f)
                {
                    _sprite.Draw(_spritesTex, new Vector2(66f + i * 32f, 66f),
                        new Rectangle(i * 32, 192, (int)(32f * ta), 32), new Color(new Vector4(1f, 0f, 0f, .75f)), r,
                        new Vector2(16f, 16f), 1.25f, SpriteEffects.None, 1f);
                }

                ta = prog - i;
                if (ta > 1f)
                    ta = 1f;

                if (ta > 0f)
                {
                    _sprite.Draw(_spritesTex, new Vector2(66f + i * 32f, 66f), new Rectangle(i * 32, 192, (int)(32f * ta), 32),
                        new Color(new Vector4(.9f, 0f, 0f, 1f)), r, new Vector2(16f, 16f), 1.25f, SpriteEffects.None, 1f);
                }
            }

            var a = _map.GetTransVal();
            if (a > 0f)
            {
                _sprite.Draw(_nullTex, new Rectangle(0, 0, (int)Game1.ScreenSize.X, (int)Game1.ScreenSize.Y),
                    new Color(new Vector4(0f, 0f, 0f, a)));
            }
            _sprite.End();
        }
    }
}