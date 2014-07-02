using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exercise3.Units
{
    public class UnitsManager : IEnumerable<IUnit>
    {
        private readonly List<IUnit> _units;

        public UnitsManager()
        {
            _units = new List<IUnit>();
        }

        public void Add(IUnit unit)
        {
            _units.Add(unit);
        }

        public IEnumerator<IUnit> GetEnumerator()
        {
            return _units.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}