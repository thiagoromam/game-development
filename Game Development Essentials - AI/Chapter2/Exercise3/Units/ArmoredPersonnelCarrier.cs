using System;
using Exercise3.Structures;

namespace Exercise3.Units
{
    public class ArmoredPersonnelCarrier : IUnit
    {
        public Type Requeriment
        {
            get { return typeof(VehicleFactory); }
        }
        public int ProduceTime
        {
            get { return 2; }
        }
        public string Group { get; set; }
    }
}