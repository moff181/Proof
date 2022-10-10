using Proof.Entities.Components;
using Proof.Render;
using System;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class RenderableComponentPanel : UserControl
    {
        private readonly RenderableComponent _renderableComponent;
        private readonly ModelLibrary _modelLibrary;
        private readonly Action<IComponent> _onRemove;

        public RenderableComponentPanel(RenderableComponent renderableComponent, ModelLibrary modelLibrary, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _renderableComponent = renderableComponent;
            _modelLibrary = modelLibrary;
            _onRemove = onRemove;

            Model.Text = _renderableComponent.Model.Path;
            Layer.Value = _renderableComponent.Layer;
        }

        public void Layer_OnValueChange()
        {
            _renderableComponent.Layer = Layer.Value;
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_renderableComponent);
        }

        private void Model_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Model? model = _modelLibrary.Get(Model.Text);
            if(model == null)
            {
                Model.Text = _renderableComponent.Model.Path;
                return;
            }

            _renderableComponent.SetModel(model);
         }
    }
}
