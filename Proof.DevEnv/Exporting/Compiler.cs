using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proof.DevEnv.Exporting
{
    public class Compiler
    {
        public void Compile(string outputDllName, string[] files)
        {
            SyntaxTree[] syntaxTrees =
                files
                .Select(file => GetSyntaxTree(file))
                .ToArray();

            List<PortableExecutableReference> references = GetNetCoreDefaultReferences();
            AddAssembly("Proof.dll", references);
            AddAssembly("Proof.OpenGL.dll", references);
            AddAssembly("GLFW.NET.dll", references);
            AddAssembly("System.Numerics.dll", references);
            AddAssembly("System.Numerics.Vectors.dll", references);

            var compilation = CSharpCompilation.Create("Executor.cs")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release))
                .WithReferences(references)
                .AddSyntaxTrees(syntaxTrees);

            using Stream codeStream = File.Create(outputDllName);

            // Can get the result here if we need to
            compilation.Emit(codeStream);
        }

        private SyntaxTree GetSyntaxTree(string file)
        {
            string src = File.ReadAllText(file);

            return SyntaxFactory.ParseSyntaxTree(src.Trim());
        }

        private List<PortableExecutableReference> GetNetCoreDefaultReferences()
        {
            var references = new List<PortableExecutableReference>();

            var rtPath = Path.GetDirectoryName(typeof(object).Assembly.Location) +
                         Path.DirectorySeparatorChar;

            AddAssemblies(
                references,
                rtPath + "System.Private.CoreLib.dll",
                rtPath + "System.Runtime.dll",
                rtPath + "System.CodeDom.dll",
                rtPath + "System.Console.dll",
                rtPath + "netstandard.dll",

                rtPath + "System.Text.RegularExpressions.dll", // IMPORTANT!
                rtPath + "System.Linq.dll",
                rtPath + "System.Linq.Expressions.dll", // IMPORTANT!

                rtPath + "System.IO.dll",
                rtPath + "System.Net.Primitives.dll",
                rtPath + "System.Net.Http.dll",
                rtPath + "System.Private.Uri.dll",
                rtPath + "System.Reflection.dll",
                rtPath + "System.ComponentModel.Primitives.dll",
                rtPath + "System.Globalization.dll",
                rtPath + "System.Collections.Concurrent.dll",
                rtPath + "System.Collections.NonGeneric.dll",
                rtPath + "Microsoft.CSharp.dll"
            );

            return references;
        }

        private void AddAssemblies(List<PortableExecutableReference> references, params string[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddAssembly(assembly, references);
            }
        }

        private void AddAssembly(string assemblyDll, List<PortableExecutableReference> references)
        {
            if (string.IsNullOrEmpty(assemblyDll))
            {
                return;
            }

            var file = Path.GetFullPath(assemblyDll);

            if (!File.Exists(file))
            {
                // check framework or dedicated runtime app folder
                string? path = Path.GetDirectoryName(typeof(object).Assembly.Location);
                if (path == null)
                {
                    return;
                }

                file = Path.Combine(path, assemblyDll);
                if (!File.Exists(file))
                {
                    return;
                }
            }

            if (references.Any(r => r.FilePath == file))
            {
                return;
            }

            try
            {
                PortableExecutableReference reference = MetadataReference.CreateFromFile(file);
                references.Add(reference);
            }
            catch
            {
                // Just return
            }
        }
    }
}
