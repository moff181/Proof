using GLFW;
using Proof.Logging;
using Exception = System.Exception;

namespace Proof.Render
{
    internal class Window : IDisposable
    {
        private readonly ALogger _logger;
        private readonly NativeWindow _nativeWindow;

        public Window(ALogger logger, int width, int height, string title)
        {
            _logger = logger;

            logger.LogInfo($"Creating window [width: {width}, height: {height}, title: {title}]...");

            try
            {
                _nativeWindow = new NativeWindow(width, height, title);
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
            _nativeWindow.Dispose();
            _logger.LogInfo("Window disposed of successfully.");
        }

        public bool IsClosing()
        {
            return _nativeWindow.IsClosing;
        }

        public void Update()
        {
            _nativeWindow.SwapBuffers();
            Glfw.PollEvents();
        }
    }
}
