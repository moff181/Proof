using Proof.Logging;

namespace Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ALogger logger = new ConsoleLogger();
            logger.LogInfo("Test Info");
            logger.LogWarn("Test Warn");
            logger.LogError("Test Error");
            logger.LogDebug("Test Debug");
        }
    }
}
