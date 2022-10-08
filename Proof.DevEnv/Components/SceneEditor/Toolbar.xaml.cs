using Proof.Entities;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components
{
    public partial class Toolbar : UserControl
    {
        private Scene? _scene;

        public Toolbar()
        {
            InitializeComponent();
        }

        public void Init(Scene? scene)
        {
            _scene = scene;
        }

        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(_scene == null)
            {
                return;
            }

            _scene.Save(_scene.FilePath);
        }

        private void SaveAs_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(_scene == null)
            {
                return;
            }

            using var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "scene";
            dialog.Filter = "Scene file (*.scene)|*.scene";
            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filePath = dialog.FileName;
            _scene.Save(filePath);
        }
    }
}
