using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.Utility
{
    /// <summary>
    /// Interaction logic for Vector2Component.xaml
    /// </summary>
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
