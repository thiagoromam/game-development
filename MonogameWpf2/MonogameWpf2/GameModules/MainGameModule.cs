using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameWpf2.Controls;
using MonogameWpf2.Models;
using MonogameWpf2.Util;

namespace MonogameWpf2.GameModules
{
    public class MainGameModule : BaseGameModule
    {
        private readonly EffectsCollection _effects;
        private Texture2D _surge;
        private List<Stamp> _stamps;
        private Effect _cuttingEffect;

        public MainGameModule(DrawingSurface drawingSurface)
            : base(drawingSurface, "Content")
        {
            _effects = Injection.Container.Resolve<EffectsCollection>();
            _effects.PropertyChanged += (s, e) => { MouseStamp.Effect = _effects.Selected.Effect; };

            drawingSurface.Height = 480;
            drawingSurface.Width = 800;
        }

        private Stamp MouseStamp
        {
            get { return _stamps.Last(); }
        }

        public override void LoadContent()
        {
            _surge = Content.Load<Texture2D>("Images/surge");
            _stamps = new List<Stamp> { new Stamp() };

            //var textureReplaceEffect = Content.Load<Effect>("Effects/TextureReplaceEffect");
            //textureReplaceEffect.Parameters["rainbow"].SetValue(Content.Load<Texture2D>("Images/rainbow"));

            _cuttingEffect = Content.Load<Effect>("Effects/CuttingEffect");

            _effects.Collection.Add(new StampEffect("None", null));
            _effects.Collection.Add(new StampEffect("180 Rotate", Content.Load<Effect>("Effects/180RotateEffect")));
            _effects.Collection.Add(new StampEffect("Coordinate Based", Content.Load<Effect>("Effects/CoordinateBasedEffect")));
            _effects.Collection.Add(new StampEffect("Cutting", _cuttingEffect));
            _effects.Collection.Add(new StampEffect("Gradient", Content.Load<Effect>("Effects/GradientEffect")));
            _effects.Collection.Add(new StampEffect("Gray Scale", Content.Load<Effect>("Effects/GrayScaleEffect")));
            _effects.Collection.Add(new StampEffect("High Contrast", Content.Load<Effect>("Effects/HighContrastEffect")));
            _effects.Collection.Add(new StampEffect("Horizontal Mirror", Content.Load<Effect>("Effects/HorizontalMirrorEffect")));
            _effects.Collection.Add(new StampEffect("Negative", Content.Load<Effect>("Effects/NegativeEffect")));
            _effects.Collection.Add(new StampEffect("Red Channel", Content.Load<Effect>("Effects/RedChannelEffect")));
            //_effects.Collection.Add(new StampEffect("Texture Replace", textureReplaceEffect));
            _effects.Selected = _effects.Collection.First();
        }
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var stamp in _stamps)
            {
                if (stamp.Effect == _cuttingEffect)
                    _cuttingEffect.Parameters["visiblePercent"].SetValue(stamp.CuttingEffectPercent);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                if (stamp.Effect != null)
                    stamp.Effect.CurrentTechnique.Passes[0].Apply();

                SpriteBatch.Draw(_surge, stamp.Position, Color.White);

                SpriteBatch.End();
            }
        }

        // events
        public override void OnMouseMove(GameMouseEventArgs e)
        {
            _stamps[_stamps.Count - 1].Position = e.Position;
        }
        public override void OnMouseDown(GameMouseButtonEventArgs e)
        {
            _stamps.Insert(_stamps.Count - 1, new Stamp
            {
                Position = MouseStamp.Position, 
                Effect = MouseStamp.Effect,
                CuttingEffectPercent = MouseStamp.CuttingEffectPercent
            });
        }
        public override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                var currentIndex = _effects.Collection.IndexOf(_effects.Selected);

                if (e.Key == Key.Up)
                {
                    currentIndex--;
                    if (currentIndex < 0)
                        currentIndex = _effects.Collection.Count - 1;
                }
                else if (e.Key == Key.Down)
                {
                    currentIndex++;
                    if (currentIndex == _effects.Collection.Count)
                        currentIndex = 0;
                }

                _effects.Selected = _effects.Collection[currentIndex];
                MouseStamp.Effect = _effects.Selected.Effect;
            }

            if (_effects.Selected.Effect == _cuttingEffect)
            {
                if (e.Key == Key.OemMinus)
                    MouseStamp.CuttingEffectPercent -= 0.02f;
                else if (e.Key == Key.OemPlus)
                    MouseStamp.CuttingEffectPercent += 0.02f;

                MouseStamp.CuttingEffectPercent = MathHelper.Clamp(MouseStamp.CuttingEffectPercent, 0, 1);
            }
        }
    }
}