namespace Proof.Logging
{
    public abstract class ALogger
    {
        public void LogInfo(string str)
        {
            Log($"[INFO] {str}");
        }

        public void LogWarn(string str)
        {
            Log($"[WARN] {str}");
        }

        public void LogError(string str)
        {
            Log($"[ERROR] {str}");
        }

        public void LogDebug(string str)
        {
            Log($"[DEBUG] {str}");
        }

        protected abstract void Log(string str);
    }
}
