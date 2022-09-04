using Proof;
using Proof.Core.Logging;

namespace Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var application = new SandboxApplication();
            application.Run("res/scenes/TestScene.xml");
        }
    }

    internal class SandboxApplication : Application
    {
        protected override ALogger GetLogger()
        {
            return new ConsoleLogger();
        }

        protected override string GetTitle()
        {
            return "Sandbox";
        }
    }
}
