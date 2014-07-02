using System;
using Exercise3.Structures;

namespace Exercise3.Units
{
    public class Tank : IUnit
    {
        public Type Requeriment
        {
            get { return typeof(VehicleFactory); }
        }
        public int ProduceTime
        {
            get { return 3; }
        }
        public string Group { get; set; }
    }
}