using Proof.Entities.Components;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class ColourComponentPanel : UserControl
    {
        private readonly ColourComponent _colourComponent;

        public ColourComponentPanel(ColourComponent ColourComponent)
        {
            InitializeComponent();

            _colourComponent = ColourComponent;

            ColourInput.Init(_colourComponent.Colour);
        }

        public void ColourInput_Change(Vector3 newValue)
        {
            _colourComponent.Colour = newValue;
        }
    }
}
