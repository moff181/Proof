using GLFW;
using Proof.Core.Logging;
using Proof.Input;
using Proof.OpenGL;
using System.Runtime.InteropServices;
using Exception = System.Exception;

namespace Proof.Render
{
    public class Window : IDisposable
    {
        private readonly ALogger _logger;
        private readonly GLFW.Window _glfwWindow;

        public Window(ALogger logger, int width, int height, string title, bool fullScreen, IntPtr? parent = null)
        {
            _logger = logger;

            logger.LogInfo($"Creating window [width: {width}, height: {height}, title: {title}]...");

            try
            {
                if(parent != null && parent != IntPtr.Zero)
                {
                    Glfw.WindowHint(Hint.Decorated, false);
                }

                _glfwWindow = Glfw.CreateWindow(
                    width,
                    height,
                    title,
                    fullScreen ? Glfw.PrimaryMonitor : GLFW.Monitor.None,
                    GLFW.Window.None);

                if(parent != null && parent != IntPtr.Zero)
                {
                    IntPtr ptr = GetWindowHandle();
                    SetParent(ptr, parent.Value);
                }

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
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        }

        public InputManager BuildInputManager()
        {
            return new InputManager(_glfwWindow);
        }

        public void Resize(int width, int height)
        {
            Glfw.SetWindowSize(_glfwWindow, width, height);
        }

        private IntPtr GetWindowHandle()
        {
            return Native.GetWin32Window(_glfwWindow);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}
