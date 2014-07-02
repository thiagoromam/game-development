using System;
using System.Collections.Generic;
using System.Linq;
using Exercise3.Agent.States;

namespace Exercise3.Agent
{
    public class FiniteStateMachine
    {
        private readonly List<IState> _stackState;

        public FiniteStateMachine(IAgent agent, IState initialState)
        {
            _stackState = new List<IState>();
            Agent = agent;
            TransitionTo(initialState);
        }

        public IState CurrentState { get; private set; }
        public IAgent Agent { get; private set; }

        public void Update()
        {
            CurrentState.Update();
        }
        
        public void TransitionTo(IState state)
        {
            if (!(state is IdleState) && !_stackState.Contains(state))
                _stackState.Add(state);

            CurrentState = state;
            CurrentState.FiniteStateMachine = this;
            CurrentState.OnLeave = OnStateLeave;

            Log.Write("Transition to: {0}", state.GetType(), ConsoleColor.Red);

            CurrentState.OnEnter();
        }
        
        private void OnStateLeave()
        {
            _stackState.Remove(CurrentState);
            TransitionTo(_stackState.Any() ? _stackState.LastOrDefault() : new IdleState());
        }
    }
}