using System;

namespace Proof.Logging
{
    public class ConsoleLogger : ALogger
    {
        protected override void Log(string str)
        {
            Console.WriteLine(str);
        }

        protected override void SetColour(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
