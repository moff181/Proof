using Proof.Audio;
using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.Entities;
using Proof.Entities.Components.Scripts;
using Proof.Input;
using Proof.OpenGL;
using Proof.Render;
using Proof.Render.Renderer;
using Proof.Render.Textures;

namespace Proof
{
    public abstract class Application
    {
        public static bool ScriptsEnabled { get; internal set; }

        protected readonly ALogger Logger;
        private readonly IOpenGLApi _gl;
        private readonly ScriptLoader _scriptLoader;

        protected Application(
            ALogger logger,
            string title,
            bool scriptsEnabled,
            ScriptLoader scriptLoader,
            bool vsync,
            IntPtr? parentWindow = null)
        {
            ScriptsEnabled = scriptsEnabled;

            Logger = logger;
            _scriptLoader = scriptLoader;

            _gl = new OpenGLApi();
            Window = new Window(_gl, logger, 1280, 720, title, false, vsync, parentWindow ?? IntPtr.Zero);
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
                using var renderer = new Renderer(_gl, Logger, 50000, 40000);
                var soundLibrary = new SoundLibrary(Logger);
                var textureLibrary = new TextureLibrary(Logger);

                Scene = Scene.LoadFromFile(
                    _gl,
                    Logger,
                    modelLibrary,
                    renderer,
                    inputManager,
                    _scriptLoader,
                    soundLibrary,
                    textureLibrary,
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
    }
}
