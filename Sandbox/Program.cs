using Proof;
using Proof.Logging;

namespace Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var application = new SandboxApplication();
            application.Run();
        }
    }

    internal class SandboxApplication : Application
    {
        protected override ALogger GetLogger()
        {
            return new ConsoleLogger();
        }
    }
}
