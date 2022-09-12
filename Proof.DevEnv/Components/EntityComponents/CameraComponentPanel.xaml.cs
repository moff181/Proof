using Proof.Entities.Components;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class CameraComponentPanel : UserControl
    {
        private readonly CameraComponent _cameraComponent;

        public CameraComponentPanel(CameraComponent cameraComponent)
        {
            InitializeComponent();

            _cameraComponent = cameraComponent;
            SetActive(cameraComponent.Active);
        }

        private void Active_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetActive(!_cameraComponent.Active);
        }

        private void SetActive(bool active)
        {
            _cameraComponent.Active = active;
            Active.Content = active ? "Deactivate" : "Activate";
        }
    }
}
