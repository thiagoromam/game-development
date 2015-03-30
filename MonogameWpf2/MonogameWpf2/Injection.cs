using Needles;

namespace MonogameWpf2
{
    public class Injection
    {
        static Injection()
        {
            Container = NeedlesContainer.Create();
        }

        public static IContainer Container { get; private set; }
    }
}