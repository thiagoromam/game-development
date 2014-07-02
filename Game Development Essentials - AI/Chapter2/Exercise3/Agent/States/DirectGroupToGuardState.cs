namespace Exercise3.Agent.States
{
    public class DirectGroupToGuardState : State
    {
        private readonly string _group;

        public DirectGroupToGuardState(string group)
        {
            _group = @group;
        }

        public override void OnEnter() { }
        public override void Update()
        {
            Log.Write("Group {0} send to guard", _group);
            Leave();
        }
    }
}