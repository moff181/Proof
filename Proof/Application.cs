using Proof.Logging;
using Proof.Render;

namespace Proof
{
    public abstract class Application
    {
        public void Run()
        {
            ALogger logger = GetLogger();

            try
            {
                
                logger.LogInfo("Application starting...");

                using var window = new Window(logger, 1280, 720, "Title");

                while (!window.IsClosing())
                {
                    window.Update();
                }
            }
            catch(Exception e)
            {
                logger.LogError("Top level exception caught", e);
            }
        }

        protected abstract ALogger GetLogger();
    }
}
