using GLFW;
using System.Numerics;

namespace Proof.Input
{
    public class InputManager
    {
        private readonly Window _glfwWindow;

        private readonly bool[] _keysDown;
        private readonly bool[] _mouseButtonsDown;
        private Vector2 _mousePosition;

        public InputManager(Window glfwWindow)
        {
            _glfwWindow = glfwWindow;

            _keysDown = new bool[GetMaxEnumValue<Keys>() + 1];
            _mouseButtonsDown = new bool[GetMaxEnumValue<MouseButton>() + 1];
        }

        public void Update()
        {
            foreach(Keys key in Enum.GetValues<Keys>())
            {
                if(key == Keys.Unknown)
                {
                    continue;
                }

                var state = Glfw.GetKey(_glfwWindow, key);
                _keysDown[(int)key] = state == InputState.Press || state == InputState.Repeat;
            }

            foreach (MouseButton mouseButton in Enum.GetValues<MouseButton>())
            {
                var state = Glfw.GetMouseButton(_glfwWindow, mouseButton);
                _mouseButtonsDown[(int)mouseButton] = state == InputState.Press || state == InputState.Repeat;
            }

            Glfw.GetCursorPosition(_glfwWindow, out double x, out double y);
            _mousePosition = new Vector2((float)x, (float)y);
        }

        public bool IsKeyDown(Keys key)
        {
            return _keysDown[(int)key];
        }

        public bool IsMouseButtonDown(MouseButton mouseButton)
        {
            return _mouseButtonsDown[(int)mouseButton];
        }

        public Vector2 GetMousePosition()
        {
            return _mousePosition;
        }

        public string GetClipboardContents()
        {
            return Glfw.GetClipboardString(_glfwWindow);
        }

        public void SetClipboardContents(string str)
        {
            Glfw.SetClipboardString(_glfwWindow, str);
        }

        private static int GetMaxEnumValue<TEnum>()
            where TEnum : notnull, Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<int>().Max();
        }
    }
}
