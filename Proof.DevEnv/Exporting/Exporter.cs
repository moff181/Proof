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

        public static void OutputRequiredFiles(string directory)
        {
            CopyDllFiles(directory);
            CopyProofRunnerFiles(directory);
        }

        private static void CopyDllFiles(string directory)
        {
            File.Copy("glfw.dll", Path.Combine(directory, "glfw.dll"));
            File.Copy("GLFW.NET.dll", Path.Combine(directory, "GLFW.NET.dll"));
            File.Copy("Proof.dll", Path.Combine(directory, "Proof.dll"));
            File.Copy("Proof.OpenGL.dll", Path.Combine(directory, "Proof.OpenGL.dll"));
        }

        private static void CopyProofRunnerFiles(string directory)
        {
            File.Copy("Proof.Runner.dll", Path.Combine(directory, "Proof.Runner.dll"));
            File.Copy("Proof.Runner.exe", Path.Combine(directory, "Proof.Runner.exe"));
            File.Copy("Proof.Runner.runtimeconfig.json", Path.Combine(directory, "Proof.Runner.runtimeconfig.json"));
        }
    }
}
