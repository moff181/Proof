using System;

namespace Proof.Logging
{
    public abstract class ALogger
    {
        public void LogInfo(string str)
        {
            SetColour(ConsoleColor.White);
            Log($"[INFO] {str}");
        }

        public void LogWarn(string str)
        {
            SetColour(ConsoleColor.Yellow);
            Log($"[WARN] {str}");
        }

        public void LogError(string str)
        {
            SetColour(ConsoleColor.Red);
            Log($"[ERROR] {str}");
        }

        public void LogDebug(string str)
        {
            SetColour(ConsoleColor.Cyan);
            Log($"[DEBUG] {str}");
        }

        protected abstract void Log(string str);
        protected abstract void SetColour(ConsoleColor color);
    }
}
