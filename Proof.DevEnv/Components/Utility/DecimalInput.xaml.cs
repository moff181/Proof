using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.Utility
{
    public partial class DecimalInput : UserControl
    {
        public DecimalInput()
        {
            InitializeComponent();
        }

        public float Value { get { return float.Parse(Input.Text); } set { Input.Text = value.ToString(); } }
        
        public Action? OnValueChange { get; set; }

        private void Value_LostFocus(object s, System.Windows.RoutedEventArgs e)
        {
            Input.Text = FormatToNumber(Input.Text);

            if(OnValueChange == null)
            {
                return;
            }

            OnValueChange();
        }

        private void TextBox_PreviewTextInput(object s, TextCompositionEventArgs e)
        {
            if (s == null || s is not TextBox)
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
            if (string.IsNullOrWhiteSpace(s))
            {
                return "0";
            }

            s = s.Replace(" ", "");

            if (s.EndsWith("."))
            {
                s += "0";
            }

            return s;
        }
    }
}
