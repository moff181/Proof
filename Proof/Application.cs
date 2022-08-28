using Proof.Logging;

namespace Proof
{
    public abstract class Application
    {
        public void Run()
        {
            ALogger logger = GetLogger();
            logger.LogInfo("Application starting...");
        }

        protected abstract ALogger GetLogger();
    }
}
