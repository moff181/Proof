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
            XValue.Text = val.X.ToString();
            YValue.Text = val.Y.ToString();
        }

        private void Value_LostFocus(object s, System.Windows.RoutedEventArgs e)
        {
            XValue.Text = FormatToNumber(XValue.Text);
            YValue.Text = FormatToNumber(YValue.Text);

            if (OnChange == null)
            {
                return;
            }

            float x = float.Parse(XValue.Text);
            float y = float.Parse(YValue.Text);

            OnChange(new Vector2(x, y));
        }

        private void TextBox_PreviewTextInput(object s, TextCompositionEventArgs e)
        {
            if(s == null || s is not TextBox)
            {
                return;
            }

            TextBox sender = (TextBox)s;

            string newText = sender.Text.Insert(sender.SelectionStart, e.Text);

            if (newText.Count(c => c == '-') > 1)
            {
                e.Handled = true;
                return;
            }

            if (newText.Count(c => c == '-') == 1 && !newText.Replace(" ", "").StartsWith('-'))
            {
                e.Handled = true;
                return;
            }

            var regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch(newText.Replace("-", ""));
        }

        private string FormatToNumber(string s)
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                return "0";
            }

            s = s.Replace(" ", "");
            
            if(s.EndsWith("."))
            {
                s += "0";
            }

            return s;
        }
    }
}
