using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise3.Structures
{
    public class StructuresManager
    {
        private readonly List<IStructure> _structures;

        public StructuresManager()
        {
            _structures = new List<IStructure>();
        }

        public bool Has(Type type)
        {
            return _structures.Any(s => s.GetType() == type);
        }

        public void Add(IStructure structure)
        {
            _structures.Add(structure);
        }
    }
}