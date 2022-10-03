using System.IO;

namespace Proof.DevEnv.Exporting
{
    public class Exporter
    {
        private readonly Compiler _compiler;
        private readonly EntryPointGenerator _entryPointGenerator;

        public Exporter(Compiler compiler, EntryPointGenerator entryPointGenerator)
        {
            _compiler = compiler;
            _entryPointGenerator = entryPointGenerator;
        }

        public void Export(string directory, string outputDllName)
        {
            _entryPointGenerator.GenerateEntryPointFile(Path.Combine(directory, "GameApplication.cs"));
            string[] files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            _compiler.Compile(outputDllName, files);
        }
    }
}
