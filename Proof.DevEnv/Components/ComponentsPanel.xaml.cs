using Proof.Entities;
using Proof.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proof.DevEnv.Components
{
    /// <summary>
    /// Interaction logic for ComponentsPanel.xaml
    /// </summary>
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
