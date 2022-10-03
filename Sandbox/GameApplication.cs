using Proof.Core.Logging;
using Proof.Entities.Components.Scripts;
using System.Reflection;

namespace Proof.Game
{
    internal class GameApplication : Application
    {
        public GameApplication(ALogger logger)
            : base(logger, "Proof Game", new ScriptLoader(Assembly.GetExecutingAssembly(), logger))
        { }
    }
}