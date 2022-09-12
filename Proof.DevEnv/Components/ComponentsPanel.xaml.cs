using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
using System;
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

        public void Init(Entity entity)
        {
            Body.Children.Clear();

            foreach(IComponent comp in entity.GetComponents())
            {
                UIElement uiElement = comp switch
                {
                    CameraComponent cameraComp => new CameraComponentPanel(cameraComp),
                    RenderableComponent renderableComp => new RenderableComponentPanel(),
                    ScriptComponent scriptComp => new ScriptComponentPanel(),
                    TransformComponent transformComp => new TransformComponentPanel(transformComp),
                    _ => new TextBlock { Text = comp.GetType().Name }
                };

                Body.Children.Add(uiElement);
            }
        }
    }
}
