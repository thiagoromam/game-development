using Needles;

namespace MonoGame.Interop
{
    internal class MonoGameInteropInjection
    {
        static MonoGameInteropInjection()
        {
            Container = NeedlesContainer.Create();
        }

        public static IContainer Container { get; private set; } 
    }
}