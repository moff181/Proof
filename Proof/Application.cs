using Proof.Core.Logging;
using Proof.Entities;
using Proof.Input;
using Proof.Render;
using Proof.Render.Buffers;

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
                var layout = VertexLayout.LoadFromFile(logger, "res/layouts/Static.xml");
                using var shader = Shader.LoadFromFile(logger, "res/shaders/Static.xml");
                using var renderer = new Renderer(logger, 50000, 40000);

                using var scene = Scene.LoadFromFile(
                    logger,
                    shader,
                    modelLibrary,
                    renderer,
                    layout,
                    inputManager,
                    startupScene);

                uint frames = 0;
                DateTime lastUpdate = DateTime.Now;
                while (!window.ShouldClose())
                {
                    window.Update();

                    inputManager.Update();

                    shader.Bind();

                    scene.Update();

                    renderer.Flush(layout);

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
