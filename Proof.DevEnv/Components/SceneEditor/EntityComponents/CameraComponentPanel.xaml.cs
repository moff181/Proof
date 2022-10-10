using Proof.Entities.Components;
using System;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class CameraComponentPanel : UserControl
    {
        private readonly CameraComponent _cameraComponent;
        private readonly Action<IComponent> _onRemove;

        public CameraComponentPanel(CameraComponent cameraComponent, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _cameraComponent = cameraComponent;
            _onRemove = onRemove;
            SetActive(cameraComponent.Active);
        }

        private void Active_Click(object sender, System.Windows.RoutedEventArgs e)
        {
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
    }
}
