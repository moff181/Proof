using System;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.Utility
{
    public partial class Vector2Component : UserControl
    {
        public Vector2Component()
        {
            InitializeComponent();
        }

        public Action<Vector2>? OnChange { get; set; }

        public void Init(Vector2 val)
        {
            XValue.Value = val.X;
            YValue.Value = val.Y;
        }

        private void Input_OnValueChange()
        {
            if(OnChange == null)
            {
                return;
            }

            OnChange(new Vector2(XValue.Value, YValue.Value));
        }
    }
}
