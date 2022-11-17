using Proof.Entities.Components;
using System;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class CameraComponentPanel : UserControl
    {
        private readonly CameraComponent _cameraComponent;
        private readonly ChangeHistory _changeHistory;
        private readonly Action<IComponent> _onRemove;

        public CameraComponentPanel(CameraComponent cameraComponent, ChangeHistory changeHistory, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _cameraComponent = cameraComponent;
            _changeHistory = changeHistory;
            _onRemove = onRemove;
            SetActive(cameraComponent.Active);
            Shader.Text = _cameraComponent.Shader.FilePath;
        }

        private void Active_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _changeHistory.RegisterChange();
            SetActive(!_cameraComponent.Active);
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_cameraComponent);
        }

        private void SetActive(bool active)
        {
            _cameraComponent.Active = active;
            Active.Content = active ? "Deactivate" : "Activate";
        }

        private void Shader_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO
        }
    }
}
