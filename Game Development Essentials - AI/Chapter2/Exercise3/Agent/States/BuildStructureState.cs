using System;
using Exercise3.Structures;

namespace Exercise3.Agent.States
{
    public class BuildStructureState<T> : State where T : IStructure, new()
    {
        private readonly T _structure;
        private int _remainingProduceTime;

        public BuildStructureState()
        {
            _structure = new T();
        }

        public override void OnEnter()
        {
            if (CanProduce())
            {
                _remainingProduceTime = _structure.ProduceTime;
            }
            else
            {
                BuildRequeriment();
            }
        }
        public override void Update()
        {
            _remainingProduceTime--;

            if (_remainingProduceTime <= 0)
            {
                Produce();
                Leave();
            }
        }

        private bool CanProduce()
        {
            return _structure.Requeriment == null || FiniteStateMachine.Agent.StructuresManager.Has(_structure.Requeriment);
        }
        private void Produce()
        {
            FiniteStateMachine.Agent.StructuresManager.Add(_structure);
            Log.Write("{0} built.", typeof(T));
        }
        private void BuildRequeriment()
        {
            FiniteStateMachine.TransitionTo(Extensions.CreateBuildStructState(_structure.Requeriment));
        }
    }
}