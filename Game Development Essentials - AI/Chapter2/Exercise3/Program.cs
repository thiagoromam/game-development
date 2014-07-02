using System.Threading;
using Exercise3.Agent;

namespace Exercise3
{
    class Program
    {
        private static void Main()
        {
            var agent = new ComputerAgent();

            while (true)
            {
                Thread.Sleep(1000);
                agent.Update();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}
