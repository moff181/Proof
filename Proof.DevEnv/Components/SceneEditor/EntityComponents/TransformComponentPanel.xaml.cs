using Proof.Entities.Components;
using System;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class TransformComponentPanel : UserControl
    {
        private readonly TransformComponent _transformComp;
        private readonly ChangeHistory _changeHistory;
        private readonly Action<IComponent> _onRemove;

        public TransformComponentPanel(TransformComponent transformComp, ChangeHistory changeHistory, Action<IComponent> onRemove)
        {
            InitializeComponent();
            
            _transformComp = transformComp;
            _changeHistory = changeHistory;
            _onRemove = onRemove;

            Position.Init(transformComp.Position);
            Scale.Init(transformComp.Scale);
        }

        public void PositionChange(Vector2 newValue)
        {
            _changeHistory.RegisterChange();
            _transformComp.Position = newValue;
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove(_transformComp);
        }

        public void ScaleChange(Vector2 newValue)
        {
            _changeHistory.RegisterChange();
            _transformComp.Scale = newValue;
        }
    }
}
