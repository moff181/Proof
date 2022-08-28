using Proof.Logging;

namespace Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ALogger logger = new ConsoleLogger();
            logger.LogInfo("Test");
        }
    }
}
