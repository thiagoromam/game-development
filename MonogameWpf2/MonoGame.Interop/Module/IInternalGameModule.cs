using System;
using MonoGame.Interop.Arguments;
using MonoGame.Interop.Controls;

namespace MonoGame.Interop.Module
{
    internal interface IInternalGameModule
    {
        void Prepare(DrawingSurface drawingSurface, IServiceProvider provider);
        void Run();
        void Draw(DrawEventArgs e);
    }
}