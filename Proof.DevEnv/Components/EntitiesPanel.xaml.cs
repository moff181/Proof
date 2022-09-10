using Proof.Entities;
using System.Windows.Controls;

namespace Proof.DevEnv.Components
{
    public partial class EntitiesPanel : UserControl
    {
        private Scene? _scene;

        public EntitiesPanel()
        {
            InitializeComponent();
        }

        public void Init(Scene scene)
        {
            _scene = scene;

            // Load entities here
        }
    }
}
