namespace Exercise3.Agent.States
{
    public class DirectGroupToAttackState : State
    {
        private readonly string _group;

        public DirectGroupToAttackState(string group)
        {
            _group = group;
        }

        public override void OnEnter() {}
        public override void Update()
        {
            Log.Write("Group {0} send to attack", _group);
            Leave();
        }
    }
}