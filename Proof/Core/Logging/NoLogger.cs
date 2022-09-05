namespace Proof.Core.Logging
{
    public class NoLogger : ALogger
    {
        protected override void Log(string str)
        { }

        protected override void SetColour(ConsoleColor color)
        { }
    }
}
