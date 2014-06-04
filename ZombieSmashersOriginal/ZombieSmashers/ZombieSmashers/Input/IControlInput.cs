namespace ZombieSmashers.Input
{
    public interface IControlInput
    {
        bool KeyLeft { get; }
        bool KeyRight { get; }
        bool KeyUp { get; }
        bool KeyDown { get; }
        bool KeyJump { get; }
        bool KeyAttack { get; }
        bool KeySecondary { get; }
    }
}