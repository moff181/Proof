using Proof.Entities.Components;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ScriptComponentPanel : UserControl
    {
        private readonly ScriptComponent _scriptComponent;

        public ScriptComponentPanel(ScriptComponent scriptComponent)
        {
            InitializeComponent();

            _scriptComponent = scriptComponent;
            ScriptName.Text = _scriptComponent.ScriptName;
        }

        private void ScriptName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _scriptComponent.ScriptName = ScriptName.Text;
        }
    }
}
