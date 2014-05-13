using Funq.Fast;
using MouseLib.Api;

namespace MouseLib
{
    public static class App
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;
            _registered = true;

            var mouseControl = new MouseControl();

            DependencyInjection.Register((IMouseInput)mouseControl);
            DependencyInjection.Register((IMouseComponent)mouseControl);
            DependencyInjection.Register((IMouseDrawer)new MouseDrawer(mouseControl));
        }
    }
}