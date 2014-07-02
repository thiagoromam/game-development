using System;

namespace Exercise3.Structures
{
    public class VehicleFactory : IStructure
    {
        public Type Requeriment
        {
            get { return typeof(IronRefinery); }
        }
        public int ProduceTime
        {
            get { return 5; }
        }
    }
}