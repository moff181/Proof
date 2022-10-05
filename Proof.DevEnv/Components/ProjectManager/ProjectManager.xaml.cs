using System.IO;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components.ProjectManager
{
    /// <summary>
    /// Interaction logic for ProjectManager.xaml
    /// </summary>
    public partial class ProjectManager : UserControl
    {
        public ProjectManager()
        {
            InitializeComponent();

            using var dialog = new FolderBrowserDialog();
            ContainingFolderText.Text = dialog.SelectedPath;
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
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
        }
    }
}
