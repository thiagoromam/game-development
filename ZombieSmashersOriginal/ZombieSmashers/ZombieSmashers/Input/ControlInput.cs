namespace ZombieSmashers.Input
{
    public static class ControlInput
    {
        private static readonly GamePadControlInput GamePadControl;
        private static readonly KeyboardControlInput KeyboardControl;

        private static IControlInput _currentControl;

        static ControlInput()
        {
            GamePadControl = new GamePadControlInput();
            KeyboardControl = new KeyboardControlInput();
        }

        public static bool KeyLeft
        {
            get { return _currentControl.KeyLeft; }
        }
        public static bool KeyRight
        {
            get { return _currentControl.KeyRight; }
        }
        public static bool KeyUp
        {
            get { return _currentControl.KeyUp; }
        }
        public static bool KeyDown
        {
            get { return _currentControl.KeyDown; }
        }
        public static bool KeyJump
        {
            get { return _currentControl.KeyJump; }
        }
        public static bool KeyAttack
        {
            get { return _currentControl.KeyAttack; }
        }
        public static bool KeySecondary
        {
            get { return _currentControl.KeySecondary; }
        }

        public static void Update()
        {
            _currentControl = KeyboardControl;
            KeyboardControl.Update();
            if (HasInput(KeyboardControl))
                return;

            _currentControl = GamePadControl;
            GamePadControl.Update();
        }

        private static bool HasInput(IControlInput control)
        {
            return control.KeyLeft ||
                   control.KeyRight ||
                   control.KeyUp ||
                   control.KeyDown ||
                   control.KeyJump ||
                   control.KeyAttack ||
                   control.KeySecondary;
        }
    }
}
