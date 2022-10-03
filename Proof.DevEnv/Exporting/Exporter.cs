using System.IO;

namespace Proof.DevEnv.Exporting
{
    public class Exporter
    {
        private readonly Compiler _compiler;

        public Exporter(Compiler compiler)
        {
            _compiler = compiler;
        }

        public void Export(string directory, string outputDllName)
        {
            string[] files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            _compiler.Compile(outputDllName, files);
        }
    }
}
