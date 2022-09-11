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
                var textBlock = new TextBlock
                {
                    Text = comp.GetType().Name,
                };

                Body.Children.Add(textBlock);
            }
        }
    }
}
