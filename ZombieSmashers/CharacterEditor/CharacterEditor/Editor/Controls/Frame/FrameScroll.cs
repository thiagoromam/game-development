using System;
using CharacterEditor.Character;
using CharacterEditor.Ioc.Api.Settings;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;
using Helpers;
using Microsoft.Xna.Framework.Graphics;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.Frame
{
    public interface IFrameScroll
    {
        int ScrollIndex { get; }
        int Limit { get; }
        bool IsCurrentFrameVisible();
        bool IsFrameVisible(int frameIndex);
        event Action ScrollIndexChanged;
    }

    public class FrameScroll : IFrameScroll, IControlComponent, IControl
    {
        private readonly IconButton _scrollUpButton;
        private readonly IconButton _scrollDownButton;
        private readonly int _scrollLimit;
        private readonly IReadonlySettings _settings;
        private int _scrollIndex;
        private readonly int _limit;

        public FrameScroll(int limit, int x, int y)
        {
            _limit = limit;
            _scrollUpButton = new IconButton(1, x, y) { Click = ScrollUp };
            _scrollDownButton = new IconButton(2, x, y + 290) { Click = ScrollDown };
            _settings = DependencyInjection.Resolve<IReadonlySettings>();
            _scrollLimit = DependencyInjection.Resolve<CharacterDefinition>().Frames.Length - limit;
        }

        public int ScrollIndex
        {
            get { return _scrollIndex; }
            private set
            {
                if (value == _scrollIndex)
                    return;

                _scrollIndex = value;
                OnScrollIndexChanged();
            }
        }
        public int Limit
        {
            get { return _limit; }
        }

        public event Action ScrollIndexChanged;

        public bool IsFrameVisible(int frameIndex)
        {
            return frameIndex.Between(_scrollIndex, _scrollIndex + _limit);
        }
        public bool IsCurrentFrameVisible()
        {
            return IsFrameVisible(_settings.SelectedFrameIndex);
        }
        private void OnScrollIndexChanged()
        {
            var handler = ScrollIndexChanged;
            if (handler != null) handler();
        }
        private void ScrollUp()
        {
            if (ScrollIndex > 0)
                ScrollIndex--;
        }
        private void ScrollDown()
        {
            if (ScrollIndex < _scrollLimit)
                ScrollIndex++;
        }

        public void Update()
        {
            _scrollUpButton.Update();
            _scrollDownButton.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _scrollUpButton.Draw(spriteBatch);
            _scrollDownButton.Draw(spriteBatch);
        }
    }
}