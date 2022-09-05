using Proof.Core.Logging;
using Proof.Entities;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;
using System.Runtime.InteropServices;

namespace Proof
{
    public abstract class Application
    {
        public void Run(string startupScene)
        {
            ALogger logger = GetLogger();

            try
            {
                logger.LogInfo("Application starting...");

                using var window = new Window(logger, 1280, 720, GetTitle(), false);
                InputManager inputManager = window.BuildInputManager();

                var modelLibrary = new ModelLibrary(logger);
                using var renderer = new Renderer(logger, 50000, 40000);

                using var scene = Scene.LoadFromFile(
                    logger,
                    modelLibrary,
                    renderer,
                    inputManager,
                    startupScene);

                uint frames = 0;
                DateTime lastUpdate = DateTime.Now;
                while (!window.ShouldClose())
                {
                    window.Update();

                    inputManager.Update();

                    scene.Update();

                    frames++;
                    if((DateTime.Now - lastUpdate).TotalMilliseconds >= 1000)
                    {
                        logger.LogDebug($"{frames} fps ({Math.Round(1000.0f / frames, 2)} ms/frame)");

                        frames = 0;
                        lastUpdate = DateTime.Now;
                    }
                }
            }
            catch(System.Exception e)
            {
                logger.LogError("Top level exception caught", e);
            }
        }

        protected abstract ALogger GetLogger();
        protected abstract string GetTitle();
    }
}
