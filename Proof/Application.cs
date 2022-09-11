using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.Entities;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.Render;

namespace Proof
{
    public abstract class Application
    {
        protected readonly ALogger Logger;

        public Application(ALogger logger)
        {
            Logger = logger;
            Window = new Window(logger, 1280, 720, GetTitle(), false, GetParentWindow());
            GlQueue = new Queue<Action>();
            Scene = null;
        }

        public Window Window { get; }
        public Queue<Action> GlQueue { get; }
        public Scene? Scene { get; internal set; }

        public void Run(string startupScene)
        {
            try
            {
                Logger.LogInfo("Application starting...");
                InputManager inputManager = Window.BuildInputManager();

                var modelLibrary = new ModelLibrary(Logger);
                using var renderer = new Renderer(Logger, 50000, 40000);

                Scene = Scene.LoadFromFile(
                    Logger,
                    modelLibrary,
                    renderer,
                    inputManager,
                    GetScriptLoader(),
                    startupScene);

                uint frames = 0;
                DateTime lastUpdate = DateTime.Now;
                while (!Window.ShouldClose())
                {
                    GlQueue.ForEachDequeue(a => a());
                    Window.Update();

                    inputManager.Update();

                    Scene.Update();

                    frames++;
                    if((DateTime.Now - lastUpdate).TotalMilliseconds >= 1000)
                    {
                        Logger.LogDebug($"{frames} fps ({Math.Round(1000.0f / frames, 2)} ms/frame)");

                        frames = 0;
                        lastUpdate = DateTime.Now;
                    }
                }
            }
            catch(Exception e)
            {
                Logger.LogError("Top level exception caught", e);
            }
        }

        protected abstract string GetTitle();
        protected abstract IScriptLoader GetScriptLoader();

        protected virtual IntPtr GetParentWindow()
        {
            return IntPtr.Zero;
        }
    }
}
