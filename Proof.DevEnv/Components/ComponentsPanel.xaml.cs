using Proof.DevEnv.Components.EntityComponents;
using Proof.Entities;
using Proof.Entities.Components;
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
                switch(comp.GetType().Name)
                {
                    case "CameraComponent":
                        Body.Children.Add(new CameraComponentPanel());
                        break;
                    case "RenderableComponent":
                        Body.Children.Add(new RenderableComponentPanel());
                        break;
                    case "ScriptComponent":
                        Body.Children.Add(new ScriptComponentPanel());
                        break;
                    case "TransformComponent":
                        Body.Children.Add(new TransformComponentPanel());
                        break;
                    default:
                        var textBlock = new TextBlock
                        {
                            Text = comp.GetType().Name,
                        };

                        Body.Children.Add(textBlock);
                        break;
                }
            }
        }
    }
}
