using GLFW;
using Proof.Core.Logging;
using Proof.Input;
using Proof.OpenGL;
using Exception = System.Exception;

namespace Proof.Render
{
    public class Window : IDisposable
    {
        private readonly ALogger _logger;
        private readonly GLFW.Window _glfwWindow;

        public Window(ALogger logger, int width, int height, string title, bool fullScreen)
        {
            _logger = logger;

            logger.LogInfo($"Creating window [width: {width}, height: {height}, title: {title}]...");

            try
            {
                _glfwWindow = Glfw.CreateWindow(
                    width,
                    height,
                    title,
                    fullScreen ? Glfw.PrimaryMonitor : GLFW.Monitor.None,
                    GLFW.Window.None);

                _logger.LogInfo("Binding GL imports...");
                Glfw.MakeContextCurrent(_glfwWindow);
                GL.Import(Glfw.GetProcAddress);
                _logger.LogInfo("GL binding completed.");
            }
            catch(Exception e)
            {
                throw new Exception("An exception occurred while creating the window", e);
            }

            logger.LogInfo("Window created.");
        }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of window...");
            Glfw.Terminate();
            _logger.LogInfo("Window disposed of successfully.");
        }

        public bool ShouldClose()
        {
            return Glfw.WindowShouldClose(_glfwWindow);
        }

        public void Update()
        {
            Glfw.SwapBuffers(_glfwWindow);
            Glfw.PollEvents();
        }

        public InputManager BuildInputManager()
        {
            return new InputManager(_glfwWindow);
        }
    }
}
