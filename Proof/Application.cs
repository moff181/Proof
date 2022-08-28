using Proof.Logging;
using Proof.Render;

namespace Proof
{
    public abstract class Application
    {
        public void Run()
        {
            ALogger logger = GetLogger();

            logger.LogInfo("Application starting...");

            using var window = new Window(logger, 1280, 720, "Title");

            while (!window.IsClosing())
            {
                window.Update();
            }
        }

        protected abstract ALogger GetLogger();
    }
}
