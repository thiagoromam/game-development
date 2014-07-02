using System;

namespace Exercise3.Agent
{
    public abstract class State : IState
    {
        private Action _onLeave;

        public FiniteStateMachine FiniteStateMachine { protected get; set; }
        public abstract void OnEnter();
        public abstract void Update();
        Action IState.OnLeave
        {
            set { _onLeave = value; }
        }
        protected void Leave()
        {
            _onLeave();
        }
    }
}