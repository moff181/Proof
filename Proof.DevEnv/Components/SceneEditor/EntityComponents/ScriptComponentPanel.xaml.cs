using Proof.Entities.Components;
using Proof.Entities.Components.Scripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ScriptComponentPanel : UserControl
    {
        private readonly ScriptComponent _scriptComponent;
        private readonly ScriptLoader _scriptLoader;
        private readonly Action<IComponent> _onRemove;

        public ScriptComponentPanel(ScriptComponent scriptComponent, ScriptLoader scriptLoader, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _scriptComponent = scriptComponent;
            _scriptLoader = scriptLoader;
            _onRemove = onRemove;

            ScriptName.Text = _scriptComponent.ScriptName;
            LoadScript();
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_scriptComponent);
        }

        private void ScriptName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _scriptComponent.ScriptName = ScriptName.Text;
            LoadScript();
        }

        private void LoadScript()
        {
            if(_scriptComponent == null)
            {
                return;
            }

            _scriptComponent.LoadScript();

            LoadProperties();
        }

        private void LoadProperties()
        {
            if(_scriptComponent == null)
            {
                return;
            }

            var properties = _scriptComponent.GetProperties();

            Properties.Children.Clear();

            foreach(KeyValuePair<string, object?> property in properties)
            {
                var output = new TextBlock()
                {
                    Text = property.Key,
                };
                Properties.Children.Add(output);
            }
        }
    }
}
