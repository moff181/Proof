using Proof.Entities.Components;
using System;
using System.Collections.Generic;
using System.Windows;
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
            LoadScript(false);
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_scriptComponent);
        }

        private void ScriptName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_scriptComponent.ScriptName == ScriptName.Text)
            {
                return;
            }

            _scriptComponent.ScriptName = ScriptName.Text;
            LoadScript(true);
        }

        private void LoadScript(bool reloadIfLoaded)
        {
            if(_scriptComponent == null)
            {
                return;
            }

            if (!_scriptComponent.IsLoaded() || reloadIfLoaded)
            {
                _scriptComponent.LoadScript();
            }

            LoadProperties();
        }

        private void LoadProperties()
        {
            Properties.Children.Clear();

            if (_scriptComponent == null || !_scriptComponent.IsLoaded())
            {
                return;
            }

            var properties = _scriptComponent.GetProperties();

            foreach(KeyValuePair<string, object?> property in properties)
            {
                var dockPanel = new DockPanel
                {
                    LastChildFill = true,
                };

                var text = new TextBlock
                {
                    Text = property.Key,
                    FontWeight = FontWeights.Bold,
                };
                dockPanel.Children.Add(text);

                var value = new TextBox
                {
                    Text = property.Value?.ToString() ?? string.Empty,
                };
                value.LostKeyboardFocus += (sender, e) =>
                {
                    bool worked = _scriptComponent.SetProperty(property.Key, value.Text);

                    if (!worked)
                    {
                        value.Text = _scriptComponent.GetProperty(property.Key)?.ToString() ?? string.Empty;
                    }
                };
                dockPanel.Children.Add(value);

                Properties.Children.Add(dockPanel);
            }
        }
    }
}
