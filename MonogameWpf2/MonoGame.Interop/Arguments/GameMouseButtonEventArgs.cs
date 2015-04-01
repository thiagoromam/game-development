using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using MonoGame.Interop.Helpers;

namespace MonoGame.Interop.Arguments
{
    public class GameMouseButtonEventArgs
    {
        private readonly MouseButtonEventArgs _args;
        private readonly Lazy<Vector2> _position;

        public GameMouseButtonEventArgs(MouseButtonEventArgs args, IInputElement inputElement)
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