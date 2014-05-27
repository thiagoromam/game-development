using System;
using System.Collections.Generic;
using Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace SharedLib.Gui
{
    public interface IScroll
    {
        int ScrollIndex { get; }
        int Limit { get; }
        bool IsIndexVisible(int index);
        event Action ScrollIndexChanged;
        IEnumerable<int> GetVisibleIndexes();
    }

    public class Scroll : IScroll
    {
        private readonly int _limit;
        private readonly int _scrollLimit;
        private readonly IconButton _scrollUpButton;
        private readonly IconButton _scrollDownButton;
        private int _scrollIndex;
        public event Action ScrollIndexChanged;

        public Scroll(int total, int limit, int x, int y, int yFinal)
        {
            _limit = limit;
            _scrollLimit = total - limit;
            _scrollUpButton = new IconButton(1, x, y) { Click = ScrollUp };
            _scrollDownButton = new IconButton(2, x, yFinal) { Click = ScrollDown };
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

        public bool IsIndexVisible(int index)
        {
            return index.Between(_scrollIndex, _scrollIndex + _limit);
        }
        public IEnumerable<int> GetVisibleIndexes()
        {
            for (var i = _scrollIndex; i < _scrollIndex + _limit; i++)
                yield return i;
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
