using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
using Proof.Render;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Proof.DevEnv.Components
{
    public partial class ComponentsPanel : UserControl
    {
        private Scene? _scene;
        private Entity? _entity;
        private Action? _refresh;

        public ComponentsPanel()
        {
            InitializeComponent();
        }

        public void Init(Scene scene, Entity entity, ModelLibrary modelLibrary, Action refresh)
        {
            _scene = scene;
            _entity = entity;
            _refresh = refresh;

            Body.Children.Clear();

            foreach(IComponent comp in entity.GetComponents())
            {
                UIElement uiElement = comp switch
                {
                    CameraComponent cameraComp => new CameraComponentPanel(cameraComp),
                    RenderableComponent renderableComp => new RenderableComponentPanel(renderableComp, modelLibrary),
                    ScriptComponent scriptComp => new ScriptComponentPanel(scriptComp),
                    TransformComponent transformComp => new TransformComponentPanel(transformComp),
                    _ => new TextBlock { Text = comp.GetType().Name }
                };

                Body.Children.Add(uiElement);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_scene == null || _entity == null || _refresh == null)
            {
                return;
            }

            _scene.Entities.Remove(_entity);
            _refresh();
        }
    }
}
