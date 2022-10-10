using Proof.Entities.Components;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ScriptComponentPanel : UserControl
    {
        private readonly ScriptComponent _scriptComponent;
        private readonly Assembly _scriptAssembly;
        private readonly Action<IComponent> _onRemove;

        public ScriptComponentPanel(ScriptComponent scriptComponent, Assembly scriptAssembly, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _scriptComponent = scriptComponent;
            _scriptAssembly = scriptAssembly;
            _onRemove = onRemove;

            ScriptName.Text = _scriptComponent.ScriptName;
            LoadProperties();
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_scriptComponent);
        }

        private void ScriptName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _scriptComponent.ScriptName = ScriptName.Text;
            LoadProperties();
        }

        private void LoadProperties()
        {
            Type? type = _scriptAssembly.GetType(_scriptComponent.ScriptName);
            if(type == null)
            {
                return;
            }

            Properties.Children.Clear();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(PropertyInfo property in properties)
            {
                var output = new TextBlock()
                {
                    Text = property.Name,
                };
                Properties.Children.Add(output);
            }
        }
    }
}
