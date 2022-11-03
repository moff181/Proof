using System.IO;
using System.Reflection;

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

        public void BuildGameDll(string directory, string outputDllName)
        {
            OutputRequiredFiles(directory);

            _entryPointGenerator.GenerateEntryPointFile(Path.Combine(directory, "GameApplication.cs"));
            string[] files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

            _compiler.Compile(outputDllName, files);
        }

        public static void OutputRequiredFiles(string directory)
        {
            string? sourceDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if(sourceDirectory == null)
            {
                return;
            }

            CopyDllFiles(directory, sourceDirectory);
            CopyProofRunnerFiles(directory, sourceDirectory);
        }

        private static void CopyDllFiles(string directory, string sourceDirectory)
        {
            File.Copy(Path.Combine(sourceDirectory, "glfw.dll"), Path.Combine(directory, "glfw.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "GLFW.NET.dll"), Path.Combine(directory, "GLFW.NET.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "Proof.dll"), Path.Combine(directory, "Proof.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "Proof.OpenGL.dll"), Path.Combine(directory, "Proof.OpenGL.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.Asio.dll"), Path.Combine(directory, "NAudio.Asio.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.Core.dll"), Path.Combine(directory, "NAudio.Core.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.dll"), Path.Combine(directory, "NAudio.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.Midi.dll"), Path.Combine(directory, "NAudio.Midi.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.Wasapi.dll"), Path.Combine(directory, "NAudio.Wasapi.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "NAudio.WinMM.dll"), Path.Combine(directory, "NAudio.WinMM.dll"), true);
        }

        private static void CopyProofRunnerFiles(string directory, string sourceDirectory)
        {
            File.Copy(Path.Combine(sourceDirectory, "Proof.Runner.dll"), Path.Combine(directory, "Proof.Runner.dll"), true);
            File.Copy(Path.Combine(sourceDirectory, "Proof.Runner.exe"), Path.Combine(directory, "Proof.Runner.exe"), true);
            File.Copy(Path.Combine(sourceDirectory, "Proof.Runner.runtimeconfig.json"), Path.Combine(directory, "Proof.Runner.runtimeconfig.json"), true);
        }
    }
}
