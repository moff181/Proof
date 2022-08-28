using Proof.Core.Logging;
using Proof.OpenGL;
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

                using var window = new Window(logger, 1280, 720, "Title", false);

                while (!window.ShouldClose())
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
