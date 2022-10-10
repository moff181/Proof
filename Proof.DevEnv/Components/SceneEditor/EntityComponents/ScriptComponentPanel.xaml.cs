using Proof.Entities.Components;
using System;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ScriptComponentPanel : UserControl
    {
        private readonly ScriptComponent _scriptComponent;
        private readonly Action<IComponent> _onRemove;

        public ScriptComponentPanel(ScriptComponent scriptComponent, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _scriptComponent = scriptComponent;
            _onRemove = onRemove;

            ScriptName.Text = _scriptComponent.ScriptName;
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_scriptComponent);
        }

        private void ScriptName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _scriptComponent.ScriptName = ScriptName.Text;
        }
    }
}
