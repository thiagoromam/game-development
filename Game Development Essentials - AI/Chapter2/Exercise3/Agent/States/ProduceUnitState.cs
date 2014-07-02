using Exercise3.Units;

namespace Exercise3.Agent.States
{
    public class ProduceUnitState<T> : State where T : IUnit, new()
    {
        private readonly T _unit;
        private int _remainingProduceTime;

        public ProduceUnitState()
        {
            _unit = new T();
        }

        public override void OnEnter()
        {
            if (CanProduce())
            {
                _remainingProduceTime = _unit.ProduceTime;
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
            return _unit.Requeriment == null || FiniteStateMachine.Agent.StructuresManager.Has(_unit.Requeriment);
        }
        private void Produce()
        {
            FiniteStateMachine.Agent.UnitsManager.Add(_unit);
            Log.Write("{0} produced.", typeof(T));
        }
        private void BuildRequeriment()
        {
            FiniteStateMachine.TransitionTo(Extensions.CreateBuildStructState(_unit.Requeriment));
        }
    }
}