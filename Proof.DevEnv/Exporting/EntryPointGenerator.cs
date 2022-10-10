using System.IO;

namespace Proof.DevEnv.Exporting
{
    public class EntryPointGenerator
    {
        private const string EntryPoint = @"using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System.Reflection;

namespace Proof.Game
{
    internal class GameApplication : Application
    {
        public GameApplication(ALogger logger)
            : base(logger, ""{TITLE}"", true, new ScriptLoader(Assembly.GetExecutingAssembly(), logger), false)
        { }
    }
}";

        private readonly string _title;

        public EntryPointGenerator(string title)
        {
            _title = title;
        }

        public void GenerateEntryPointFile(string fileName)
        {
            string formatted = EntryPoint.Replace("{TITLE}", _title);

            File.WriteAllText(fileName, formatted);
        }
    }
}
