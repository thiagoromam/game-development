using System;

namespace Exercise3.Structures
{
    public class IronRefinery : IStructure
    {
        public Type Requeriment
        {
            get { return typeof(PowerPlant); }
        }
        public int ProduceTime
        {
            get { return 4; }
        }
    }
}