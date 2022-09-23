using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.Render;
using System.Windows;
using System.Windows.Controls;

namespace Proof.DevEnv.Components
{
    public partial class ComponentsPanel : UserControl
    {
        public ComponentsPanel()
        {
            InitializeComponent();
        }

        public void Init(Entity entity, ModelLibrary modelLibrary)
        {
            Body.Children.Clear();

            foreach(IComponent comp in entity.GetComponents())
            {
                UIElement uiElement = comp switch
                {
                    CameraComponent cameraComp => new CameraComponentPanel(cameraComp),
                    RenderableComponent renderableComp => new RenderableComponentPanel(renderableComp, modelLibrary),
                    ScriptComponent scriptComp => new ScriptComponentPanel(),
                    TransformComponent transformComp => new TransformComponentPanel(transformComp),
                    _ => new TextBlock { Text = comp.GetType().Name }
                };

                Body.Children.Add(uiElement);
            }
        }
    }
}
