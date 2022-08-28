using System;
using System.Text;

namespace Proof.Logging
{
    public abstract class ALogger
    {
        public void LogInfo(string str)
        {
            SetColour(ConsoleColor.White);
            Log($"[INFO] {str}");
            SetColour(ConsoleColor.White);
        }

        public void LogWarn(string str)
        {
            SetColour(ConsoleColor.Yellow);
            Log($"[WARN] {str}");
            SetColour(ConsoleColor.White);
        }

        public void LogError(string str)
        {
            SetColour(ConsoleColor.Red);
            Log($"[ERROR] {str}");
            SetColour(ConsoleColor.White);
        }

        public void LogError(string str, Exception e)
        {
            var sb = new StringBuilder();
            sb.Append(str);

            Exception? innerException = e;
            do
            {
                sb.Append($" - {innerException.Message}");
            } while ((innerException = innerException?.InnerException) != null);

            LogError(sb.ToString());
        }

        public void LogDebug(string str)
        {
            SetColour(ConsoleColor.Cyan);
            Log($"[DEBUG] {str}");
            SetColour(ConsoleColor.White);
        }

        protected abstract void Log(string str);
        protected abstract void SetColour(ConsoleColor color);
    }
}
