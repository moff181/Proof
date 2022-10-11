using System.IO;
using System.Reflection;

namespace Proof.Core
{
    public class AssemblyWrapper
    {
        private readonly string _gameDllPath;

        public AssemblyWrapper(string gameDllPath)
        {
            _gameDllPath = gameDllPath;
            Assembly = Reload();
        }

        public Assembly Assembly { get; private set; }

        public Assembly Reload()
        {
            Assembly = Assembly.Load(File.ReadAllBytes(_gameDllPath));
            return Assembly;
        }
    }
}
