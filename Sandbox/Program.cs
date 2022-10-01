using Proof;
using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;

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
        public SandboxApplication() 
            : base(new ConsoleLogger(), "Sandbox", new ScriptLoader(new ConsoleLogger()))
        { }
    }
}
