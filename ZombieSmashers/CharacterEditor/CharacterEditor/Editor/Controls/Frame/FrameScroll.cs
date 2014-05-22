using System;
using CharacterEditor.Character;
using Funq.Fast;
using GraphicalUserInterfaceLib.Api;
using Microsoft.Xna.Framework.Graphics;
using SharedLib.Gui;

namespace CharacterEditor.Editor.Controls.Frame
{
    public interface IFrameScroll
    {
        int ScrollIndex { get; }
        event Action ScrollIndexChanged;
    }

    public class FrameScroll : IFrameScroll, IControlComponent, IControl
    {
        private readonly IconButton _scrollUpButton;
        private readonly IconButton _scrollDownButton;
        private readonly int _scrollLimit;
        private int _scrollIndex;

        public FrameScroll(int x, int y)
        {
            _scrollUpButton = new IconButton(1, x, y) { Click = ScrollUp };
            _scrollDownButton = new IconButton(2, x, y + 290) { Click = ScrollDown };
            _scrollLimit = DependencyInjection.Resolve<CharacterDefinition>().Frames.Length - 20;
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

        public event Action ScrollIndexChanged;

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