namespace Proof.Core.Logging
{
    public class FileLogger : ALogger
    {
        private readonly string _fileName;

        public FileLogger(string fileName)
        {
            _fileName = fileName;
        }

        protected override void Log(string str)
        {
            File.AppendAllText(_fileName, $"{str}\n");
        }

        protected override void SetColour(ConsoleColor color)
        {
            // Colour not supported for file logger
        }
    }
}
