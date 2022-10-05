using Proof.DevEnv.ProjectStructure;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components.ProjectManager
{
    public partial class ProjectManager : UserControl
    {
        public ProjectManager()
        {
            InitializeComponent();

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
        }

        private string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
