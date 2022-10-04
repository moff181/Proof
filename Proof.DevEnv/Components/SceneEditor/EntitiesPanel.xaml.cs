using Proof.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components
{
    public partial class EntitiesPanel : UserControl
    {
        private Scene? _scene;
        private Action<Entity>? _onClick;

        public EntitiesPanel()
        {
            InitializeComponent();
        }

        public void Init(Scene scene, Action<Entity> onClick)
        {
            _scene = scene;
            _onClick = onClick;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_scene == null || _onClick == null)
            {
                return;
            }

            string name = NewEntityNameBox.Text;
            _scene.Entities.Add(new Entity(name));

            NewEntityNameBox.Text = string.Empty;
            Init(_scene, _onClick);
        }

        private void NewEntityNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                Button_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
