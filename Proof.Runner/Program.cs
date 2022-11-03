using Proof.Core.Images;
using Proof.Core.Logging;
using Proof.Core.ProjectStructure;
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
            ALogger logger = new FileLogger("debug.log");

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
                entryPoint.Invoke(gameApplication, new object[] { GetStartupScene() });
            }
            catch(Exception e)
            {
                logger.LogError("Failed to create game instance", e);
            }
        }

        private static string GetStartupScene()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                .Where(x => x.EndsWith(".proof"));

            string file = files.First();

            ProgramFile programFile = ProgramFile.Load(file) ?? throw new IOException($"Could not load program file from {file}");
            return programFile.StartupScene;
        }
    }
}