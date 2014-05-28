using System;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CharacterEditor.Board
{
    public class CharacterAnimation
    {
        private readonly IReadOnlySettings _settings;
        private float _currentFrameTime;
        private readonly Vector2 _animationPosition;
        private readonly FlipTextButton<bool> _animationButton;

        public CharacterAnimation()
        {
            _settings = DependencyInjection.Resolve<IReadOnlySettings>();
            _animationPosition = new Vector2(500, 100);

            _animationButton = new FlipTextButton<bool>(550, 3);
            _animationButton.AddOption(false, "play");
            _animationButton.AddOption(true, "stop");
            _animationButton.Value = false;
        }

        private int CurrentKeyFrameIndex { get; set; }

        public void Update(GameTime gameTime)
        {
            _animationButton.Update();

            var keyFrame = _settings.SelectedAnimation.KeyFrames[CurrentKeyFrameIndex];

            if (_animationButton.Value)
            {
                _currentFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 30.0f;

                if (_currentFrameTime > keyFrame.Duration)
                {
                    _currentFrameTime -= keyFrame.Duration;
                    CurrentKeyFrameIndex++;
                    if (CurrentKeyFrameIndex >= _settings.SelectedAnimation.KeyFrames.Length) CurrentKeyFrameIndex = 0;

                    keyFrame = _settings.SelectedAnimation.KeyFrames[CurrentKeyFrameIndex];
                }
            }
            else
                CurrentKeyFrameIndex = _settings.SelectedKeyFrameIndex;

            if (keyFrame.FrameReference < 0)
                CurrentKeyFrameIndex = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var frameIndex = Math.Max(_settings.SelectedAnimation.KeyFrames[CurrentKeyFrameIndex].FrameReference, 0);

            BoardExtensions.DrawCharacterFrame(spriteBatch, _animationPosition, 0.5f, true, frameIndex, true, 1);
            
            _animationButton.Draw();
            //if (_playing)
            //{
            //    if (LegacySuport.DrawClickText(550, 3, "stop"))
            //        _playing = false;
            //}
            //else
            //{
            //    if (LegacySuport.DrawClickText(550, 3, "play"))
            //        _playing = true;
            //}
        }
    }
}