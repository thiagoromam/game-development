using System;

namespace Exercise3.Units
{
    public interface IUnit
    {
        Type Requeriment { get; }
        int ProduceTime { get; }
        string Group { get; set; }
    }
}