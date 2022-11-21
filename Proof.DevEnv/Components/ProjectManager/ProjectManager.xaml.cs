using Proof.Core.Logging;
using Proof.Core.ProjectStructure;
using Proof.DevEnv.Exporting;
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
        private readonly Action<string, string, string> _switchViewToSceneEditor;

        public ProjectManager(Action<string, string, string> switchViewToSceneEditor)
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
            Directory.CreateDirectory(Path.Combine(directory, "res", "audio"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "models"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "scenes"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "shaders"));
            Directory.CreateDirectory(Path.Combine(directory, "res", "textures"));

            var programFile = ProgramFile.CreateDefault();
            programFile.Save(Path.Combine(directory, $"{ProjectNameText.Text}.proof"));

            string filePath = Path.Combine(directory, programFile.StartupScene);
            var scene = new Scene(new NoLogger(), new NoRenderer(), filePath);
            scene.Save(filePath);

            CopyAudioFiles(directory);
            CopyModelFiles(directory);
            CopyShaderFiles(directory);
            CopyTextureFiles(directory);
            Exporter.OutputRequiredFiles(directory);

            string projectName = Path.GetFileNameWithoutExtension(filePath);
            _switchViewToSceneEditor(directory, programFile.StartupScene, projectName);
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
            
            _switchViewToSceneEditor(fileInfo.Directory.FullName, programFile.Value.StartupScene, Path.GetFileNameWithoutExtension(fileInfo.Name));
        }

        private static void CopyAudioFiles(string directory)
        {
            File.Copy(
                Path.Combine("defaults", "silence.wav"),
                Path.Combine(directory, "res", "audio", "silence.wav"));
        }

        private static void CopyModelFiles(string directory)
        {
            File.Copy(
                Path.Combine("defaults", "Square.model"),
                Path.Combine(directory, "res", "models", "Square.model"));
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

            File.Copy(
                Path.Combine("defaults", "UI.vertex"),
                Path.Combine(directory, "res", "shaders", "UI.vertex"));

            File.Copy(
                Path.Combine("defaults", "UI.frag"),
                Path.Combine(directory, "res", "shaders", "UI.frag"));

            File.Copy(
                Path.Combine("defaults", "UI.xml"),
                Path.Combine(directory, "res", "shaders", "UI.xml"));
        }

        private static void CopyTextureFiles(string directory)
        {
            File.Copy(
                Path.Combine("defaults", "no_image.png"),
                Path.Combine(directory, "res", "textures", "no_image.png"));
        }

        private static string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
