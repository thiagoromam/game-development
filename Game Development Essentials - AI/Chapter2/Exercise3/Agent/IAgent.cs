using Exercise3.Structures;
using Exercise3.Units;

namespace Exercise3.Agent
{
    public interface IAgent
    {
        StructuresManager StructuresManager { get; }
        UnitsManager UnitsManager { get; }
    }
}