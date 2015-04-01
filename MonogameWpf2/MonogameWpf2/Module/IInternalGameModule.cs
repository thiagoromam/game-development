using System;
using MonogameWpf2.Arguments;
using MonogameWpf2.Controls;

namespace MonogameWpf2.Module
{
    internal interface IInternalGameModule
    {
        void Prepare(DrawingSurface drawingSurface, IServiceProvider provider);
        void Run();
        void Draw(DrawEventArgs e);
    }
}