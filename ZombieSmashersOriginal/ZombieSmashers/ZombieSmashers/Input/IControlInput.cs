namespace ZombieSmashers.Input
{
    public interface IControlInput
    {
        bool KeyLeftPressing { get; }
        bool KeyRightPressing { get; }
        bool KeyUpPressing { get; }
        bool KeyUpPressed { get; }
        bool KeyDownPressing { get; }
        bool KeyDownPressed { get; }
        bool KeyJumpPressed { get; }
        bool KeyAttackPressed { get; }
        bool KeySecondaryPressed { get; }
        bool KeyStartPressed { get; }
    }
}