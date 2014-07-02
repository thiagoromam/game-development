using System;

namespace Exercise3.Agent
{
    public interface IState
    {
        FiniteStateMachine FiniteStateMachine { set; }
        void OnEnter();
        void Update();
        Action OnLeave { set; }
    }
}