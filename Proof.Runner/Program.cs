using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System.Reflection;

namespace Proof.Runner
{
    internal static class Program
    {
        private const string DllName = "Game.dll";
        private const string EntryPointTypeName = "Proof.Game.GameApplication";
        private const string EntryPointMethodName = "Run";

        public static void Main(string[] args)
        {
            ALogger logger = new ConsoleLogger();

            try
            {
                string directory = Directory.GetCurrentDirectory();
                string path = Path.Combine(directory, DllName);

                var assembly = Assembly.LoadFile(path);

                Type? gameApplicationType = assembly.GetType(EntryPointTypeName);
                if (gameApplicationType == null)
                {
                    throw new TypeLoadException("Failed to load GameApplication");
                }

                MethodInfo? entryPoint = gameApplicationType.GetMethod(EntryPointMethodName);
                if(entryPoint == null)
                {
                    throw new TypeLoadException("Failed to find the entry point function.");
                }

                var gameApplication = gameApplicationType.GetConstructors().First().Invoke(new object[] { logger });
                entryPoint.Invoke(gameApplication, new object[] { "res/scenes/TestScene.xml" });
            }
            catch(Exception e)
            {
                logger.LogError("Failed to create game instance", e);
            }
        }
    }
}

namespace Proof.Game
{
    internal class GameApplication : Application
    {
        public GameApplication(ALogger logger)
            : base(logger, "Proof Game", new ScriptLoader(Assembly.GetExecutingAssembly(), logger))
        { }
    }
}