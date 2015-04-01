using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using MonoGame.Interop.Helpers;

namespace MonoGame.Interop.Arguments
{
    public class GameMouseEventArgs
    {
        private readonly MouseEventArgs _args;
        private readonly Lazy<Vector2> _position;

        public GameMouseEventArgs(MouseEventArgs args, IInputElement inputElement)
        {
            _args = args;
            _position = new Lazy<Vector2>(() => _args.GetPosition(inputElement).ToVector());
        }

        public Vector2 Position
        {
            get { return _position.Value; }
        }
    }
}