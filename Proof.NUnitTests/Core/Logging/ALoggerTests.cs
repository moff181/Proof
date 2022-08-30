using Microsoft.VisualBasic;
using NUnit.Framework;
using Proof.Core.Logging;
using System.Text;

namespace Proof.NUnitTests.Core.Logging
{
    [TestFixture]
    internal class ALoggerTests
    {
        private TestLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new TestLogger();
        }

        [TestCase("")]
        [TestCase("Test 1")]
        [TestCase("Test 2")]
        [TestCase("This is a test.")]
        [TestCase("This is a test.", "This is another test.")]
        [TestCase("Test 1", "Test 2", "Test 3")]
        public void ShouldOutputDebugLogCorrectly(params string[] strings)
        {
            foreach (string str in strings)
            {
                _logger.LogDebug(str);
                Assert.AreEqual($"[DEBUG] {str}", _logger.Output.Last().Item1);
                Assert.AreEqual(ConsoleColor.Cyan, _logger.Output.Last().Item2);
            }
        }

        [TestCase("")]
        [TestCase("Test 1")]
        [TestCase("Test 2")]
        [TestCase("This is a test.")]
        [TestCase("This is a test.", "This is another test.")]
        [TestCase("Test 1", "Test 2", "Test 3")]
        public void ShouldOutputInfoLogCorrectly(params string[] strings)
        {
            foreach (string str in strings)
            {
                _logger.LogInfo(str);
                Assert.AreEqual($"[INFO] {str}", _logger.Output.Last().Item1);
                Assert.AreEqual(ConsoleColor.White, _logger.Output.Last().Item2);
            }
        }

        [TestCase("")]
        [TestCase("Test 1")]
        [TestCase("Test 2")]
        [TestCase("This is a test.")]
        [TestCase("This is a test.", "This is another test.")]
        [TestCase("Test 1", "Test 2", "Test 3")]
        public void ShouldOutputWarnLogCorrectly(params string[] strings)
        {
            foreach (string str in strings)
            {
                _logger.LogWarn(str);
                Assert.AreEqual($"[WARN] {str}", _logger.Output.Last().Item1);
                Assert.AreEqual(ConsoleColor.Yellow, _logger.Output.Last().Item2);
            }
        }

        [TestCase("")]
        [TestCase("Test 1")]
        [TestCase("Test 2")]
        [TestCase("This is a test.")]
        [TestCase("This is a test.", "This is another test.")]
        [TestCase("Test 1", "Test 2", "Test 3")]
        public void ShouldOutputErrorLogCorrectly(params string[] strings)
        {
            foreach (string str in strings)
            {
                _logger.LogError(str);
                Assert.AreEqual($"[ERROR] {str}", _logger.Output.Last().Item1);
                Assert.AreEqual(ConsoleColor.Red, _logger.Output.Last().Item2);
            }
        }

        [TestCase("Top level message", "Inner Exception 1", "Inner Exception 2")]
        [TestCase("Top level message", "Inner Exception 1", "Inner Exception 2", "Inner Exception 3")]
        [TestCase("Test 10", "Test 20", "Test 30", "Test 40")]
        public void ShouldOutputErrorLogWithExceptionCorrectly(string message, params string[] exceptionMessages)
        {
            Exception? e = null;
            foreach (string s in exceptionMessages.Reverse())
            {
                e = new Exception(s, e);
            }

            _logger.LogError(message, e);

            var sb = new StringBuilder();
            sb.Append($"[ERROR] {message}");

            foreach(string s in exceptionMessages)
            {
                sb.Append($" - {s}");
            }

            Assert.AreEqual(sb.ToString(), _logger.Output.Last().Item1);
            Assert.AreEqual(ConsoleColor.Red, _logger.Output.Last().Item2);
        }

        [TestCase("Debug string", "DEBUG", "Info string", "INFO")]
        [TestCase("Warn string", "WARN", "Error string", "ERROR")]
        [TestCase("Warn string", "WARN", "Error string", "ERROR", "Info string", "INFO")]
        [TestCase("Warn string", "WARN", "Error string", "ERROR", "Debug string", "DEBUG", "Info string", "INFO")]
        [TestCase("Error string", "ERROR", "Debug string", "DEBUG", "Info string", "INFO", "Warn string", "WARN")]
        public void ShouldOutputDifferentCallsInSeriesCorrectly(params string[] strings)
        {
            var expected = new List<Tuple<string, ConsoleColor>>();

            for(int i = 0; i < strings.Length; i += 2)
            {
                string str = strings[i];
                string type = strings[i + 1];
                switch(type)
                {
                    case "DEBUG":
                        _logger.LogDebug(str);
                        expected.Add(new Tuple<string, ConsoleColor>($"[DEBUG] {str}", ConsoleColor.Cyan));
                        break;
                    case "INFO":
                        _logger.LogInfo(str);
                        expected.Add(new Tuple<string, ConsoleColor>($"[INFO] {str}", ConsoleColor.White));
                        break;
                    case "WARN":
                        _logger.LogWarn(str);
                        expected.Add(new Tuple<string, ConsoleColor>($"[WARN] {str}", ConsoleColor.Yellow));
                        break;
                    case "ERROR":
                        _logger.LogError(str);
                        expected.Add(new Tuple<string, ConsoleColor>($"[ERROR] {str}", ConsoleColor.Red));
                        break;
                    default:
                        throw new Exception("Unexpected type");
                }
            }

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(_logger.Output[i], expected[i]);
            }
        }
    }
}
