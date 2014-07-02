using System;
using System.Collections.Generic;
using System.Linq;
using Exercise3.Agent.States;
using Exercise3.Structures;
using Exercise3.Units;

namespace Exercise3.Agent
{
    public class ComputerAgent : IAgent
    {
        private readonly FiniteStateMachine _fsm;
        private readonly List<Action> _scripts;
        private int _indexScript;
        private bool _scriptFinished;

        public ComputerAgent()
        {
            _fsm = new FiniteStateMachine(this, new IdleState());
            _scripts = new List<Action>();
            UnitsManager = new UnitsManager();
            StructuresManager = new StructuresManager();

            SetupScript();
        }

        public UnitsManager UnitsManager { get; private set; }
        public StructuresManager StructuresManager { get; private set; }

        public void Update()
        {
            _fsm.Update();

            if (_scriptFinished)
                return;

            DoScript();

            if (_scriptFinished)
                Console.WriteLine("\n\nScript finished.");
        }

        private void Produce<T>() where T : IUnit, new()
        {
            _fsm.TransitionTo(new ProduceUnitState<T>());
        }
        private void DefineGroup<T>(string group) where T : IUnit
        {
            var units = UnitsManager.OfType<T>().ToList();
            units.ForEach(u => u.Group = group);
        }
        private void DirectGroupToGuard(string group)
        {
            _fsm.TransitionTo(new DirectGroupToGuardState(group));
        }
        private void DirectGroupToAttack(string group)
        {
            _fsm.TransitionTo(new DirectGroupToAttackState(group));
        }

        private void SetupScript()
        {
            for (var i = 0; i < 4; i++)
                _scripts.Add(Produce<ArmoredPersonnelCarrier>);

            _scripts.Add(() => DefineGroup<ArmoredPersonnelCarrier>("armored personnel carriers"));
            _scripts.Add(() => DirectGroupToGuard("armored personnel carriers"));

            for (var i = 0; i < 6; i++)
                _scripts.Add(Produce<Tank>);

            _scripts.Add(() => DefineGroup<Tank>("tanks"));
            _scripts.Add(() => DirectGroupToAttack("tanks"));
        }
        private void DoScript()
        {
            if (!(_fsm.CurrentState is IdleState))
                return;

            if (_indexScript < _scripts.Count)
                _scripts[_indexScript++]();
            else
                _scriptFinished = true;
        }
    }
}