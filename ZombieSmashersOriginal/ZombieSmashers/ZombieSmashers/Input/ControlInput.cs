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

        public static bool KeyLeftPressing
        {
            get { return _currentControl.KeyLeftPressing; }
        }
        public static bool KeyRightPressing
        {
            get { return _currentControl.KeyRightPressing; }
        }
        public static bool KeyUpPressing
        {
            get { return _currentControl.KeyUpPressing; }
        }
        public static bool KeyUpPressed
        {
            get { return _currentControl.KeyUpPressed; }
        }
        public static bool KeyDownPressing
        {
            get { return _currentControl.KeyDownPressing; }
        }
        public static bool KeyDownPressed
        {
            get { return _currentControl.KeyDownPressed; }
        }
        public static bool KeyJumpPressed
        {
            get { return _currentControl.KeyJumpPressed; }
        }
        public static bool KeyAttackPressed
        {
            get { return _currentControl.KeyAttackPressed; }
        }
        public static bool KeySecondaryPressed
        {
            get { return _currentControl.KeySecondaryPressed; }
        }
        public static bool KeyStartPressed
        {
            get { return _currentControl.KeyStartPressed; }
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
            return control.KeyLeftPressing ||
                   control.KeyRightPressing ||
                   control.KeyUpPressing ||
                   control.KeyDownPressing ||
                   control.KeyJumpPressed ||
                   control.KeyAttackPressed ||
                   control.KeySecondaryPressed ||
                   control.KeyStartPressed;
        }
    }
}
