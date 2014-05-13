using Funq.Fast;
using KeyboardLib.Api;

namespace KeyboardLib
{
    public static class App
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;
            _registered = true;

            var keyboardControl = new KeyboardControl();

            DependencyInjection.Register((IKeyboardComponent)keyboardControl);
            DependencyInjection.Register((IKeyboardControl)keyboardControl);
        }
    }
}