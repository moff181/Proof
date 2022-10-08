using Proof.Core.Logging;
using Proof.Core.ProjectStructure;
using Proof.Entities;
using Proof.Render.Renderer;
using Proof.Render.Shaders;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components.ProjectManager
{
    public partial class ProjectManager : UserControl
    {
        private readonly Action<string, string> _switchViewToSceneEditor;

        public ProjectManager(Action<string, string> switchViewToSceneEditor)
        {
            InitializeComponent();

            _switchViewToSceneEditor = switchViewToSceneEditor;

            ContainingFolderText.Text = GetDesktopPath();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.InitialDirectory = ContainingFolderText.Text;
            DialogResult result = dialog.ShowDialog();

            if(result != DialogResult.OK)
            {
                return;
            }

            ContainingFolderText.Text = dialog.SelectedPath;
        }

        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            string directory = Path.Combine(ContainingFolderText.Text, ProjectNameText.Text);

            Directory.CreateDirectory(directory);

            Directory.CreateDirectory(Path.Combine(directory, "res"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "models"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "scenes"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "shaders"));

            var programFile = ProgramFile.CreateDefault();
            programFile.Save(Path.Combine(directory, $"{ProjectNameText.Text}.proof"));

            var scene = new Scene(new NoLogger(), new NoShader("res/shaders/Static.xml"), new NoRenderer());
            scene.Save(Path.Combine(directory, programFile.StartupScene));

            CopyShaderFiles(directory);
            CopyModelFiles(directory);
            CopyDllFiles(directory);
            CopyProofRunnerFiles(directory);

            _switchViewToSceneEditor(directory, programFile.StartupScene);
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "proof files (*.proof)|*.proof";
            dialog.RestoreDirectory = true;
            dialog.Multiselect = false;

            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            var fileInfo = new FileInfo(dialog.FileName);
            ProgramFile? programFile = ProgramFile.Load(fileInfo.FullName);

            if(programFile == null || fileInfo.Directory == null)
            {
                throw new IOException("Could not load program file.");
            }
            
            _switchViewToSceneEditor(fileInfo.Directory.FullName, programFile.Value.StartupScene);
        }

        private static void CopyShaderFiles(string directory)
        {
            File.Copy(
                Path.Combine("defaults", "Static.vertex"),
                Path.Combine(directory, "res", "shaders", "Static.vertex"));

            File.Copy(
                Path.Combine("defaults", "Static.frag"),
                Path.Combine(directory, "res", "shaders", "Static.frag"));

            File.Copy(
                Path.Combine("defaults", "Static.xml"),
                Path.Combine(directory, "res", "shaders", "Static.xml"));
        }

        private static void CopyModelFiles(string directory)
        {
            File.Copy(
                Path.Combine("defaults", "Square.model"),
                Path.Combine(directory, "res", "models", "Square.model"));
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

        private static string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
