using Proof.Entities.Components;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class TransformComponentPanel : UserControl
    {
        private readonly TransformComponent _transformComp;

        public TransformComponentPanel(TransformComponent transformComp)
        {
            InitializeComponent();
            
            _transformComp = transformComp;

            Position.Init(transformComp.Position);
            Scale.Init(transformComp.Scale);
        }

        public void PositionChange(Vector2 newValue)
        {
            _transformComp.Position = newValue;
        }

        public void ScaleChange(Vector2 newValue)
        {
            _transformComp.Scale = newValue;
        }
    }
}
