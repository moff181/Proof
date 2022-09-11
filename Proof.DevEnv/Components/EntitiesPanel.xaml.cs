using Proof.Entities;
using System;
using System.Windows.Controls;

namespace Proof.DevEnv.Components
{
    public partial class EntitiesPanel : UserControl
    {
        public EntitiesPanel()
        {
            InitializeComponent();
        }

        public void Init(Scene scene, Action<Entity> onClick)
        {
            Body.Children.Clear();

            foreach(Entity entity in scene.Entities)
            {
                var button = new Button
                {
                    Content = entity.Name
                };
                button.Click += (sender, e) => onClick(entity);

                Body.Children.Add(button);
            }
        }
    }
}
