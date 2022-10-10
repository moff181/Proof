using Proof.Entities.Components;
using System;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ColourComponentPanel : UserControl
    {
        private readonly ColourComponent _colourComponent;
        private readonly Action<IComponent> _onRemove;

        public ColourComponentPanel(ColourComponent ColourComponent, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _colourComponent = ColourComponent;
            _onRemove = onRemove;
            ColourInput.Init(_colourComponent.Colour);
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_colourComponent);
        }

        public void ColourInput_Change(Vector3 newValue)
        {
            _colourComponent.Colour = newValue;
        }
    }
}
