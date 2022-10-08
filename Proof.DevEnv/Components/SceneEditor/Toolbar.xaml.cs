using Proof.Entities;
using System.Windows.Controls;

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
    }
}
