using System.Reflection;

namespace Proof.Core
{
    public class AssemblyWrapper
    {
        private readonly string _gameDllPath;

        private Assembly _assembly;

        public AssemblyWrapper(string gameDllPath)
        {
            _gameDllPath = gameDllPath;
            _assembly = Reload();
        }

        public Type? GetType(string typeName)
        {
            if(_assembly == null)
            {
                return null;
            }

            return _assembly.GetType(typeName);
        }

        public Assembly Reload()
        {
            try
            {
                _assembly = Assembly.Load(File.ReadAllBytes(_gameDllPath));
            }
            catch(Exception)
            {
                // Suppress error
            }
            return _assembly;
        }
    }
}
