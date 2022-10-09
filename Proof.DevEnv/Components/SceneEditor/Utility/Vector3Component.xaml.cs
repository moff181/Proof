using System;
using System.Numerics;
using System.Windows.Controls;

namespace Proof.DevEnv.Components.Utility
{
    public partial class Vector3Component : UserControl
    {
        public Vector3Component()
        {
            InitializeComponent();
        }

        public Action<Vector3>? OnChange { get; set; }

        public void Init(Vector3 val)
        {
            XValue.Value = val.X;
            YValue.Value = val.Y;
            ZValue.Value = val.Y;
        }

        public void Input_OnValueChange()
        {
            if(OnChange == null)
            {
                return;
            }

            OnChange(new Vector3(XValue.Value, YValue.Value, ZValue.Value));
        }
    }
}
