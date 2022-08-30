using Proof.Core.Logging;

namespace Proof.NUnitTests.Core.Logging
{
    internal class TestLogger : ALogger
    {
        public List<Tuple<string, ConsoleColor>> Output { get; }

        private ConsoleColor _color;

        public TestLogger()
        {
            Output = new List<Tuple<string, ConsoleColor>>();
        }

        protected override void Log(string str)
        {
            Output.Add(new Tuple<string, ConsoleColor>(str, _color));
        }

        protected override void SetColour(ConsoleColor color)
        {
            _color = color;
        }
    }
}
