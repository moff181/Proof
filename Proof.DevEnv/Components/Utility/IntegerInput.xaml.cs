using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.Utility
{
    public partial class IntegerInput : UserControl
    {
        public IntegerInput()
        {
            InitializeComponent();
        }

        public int Value { get { return int.Parse(Input.Text); } set { Input.Text = value.ToString(); } }

        public Action? OnValueChange { get; set; }

        private void Value_LostFocus(object s, System.Windows.RoutedEventArgs e)
        {
            Input.Text = FormatToNumber(Input.Text);

            if (OnValueChange == null)
            {
                return;
            }

            OnValueChange();
        }

        private void TextBox_PreviewTextInput(object s, TextCompositionEventArgs e)
        {
            if (s is not TextBox)
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

            var regex = new Regex("^\\d+$");
            e.Handled = !regex.IsMatch(newText.Replace("-", ""));
        }

        private string FormatToNumber(string s)
        {
            s = s.Replace(" ", "");

            bool isNegative = s.StartsWith('-');

            s = s.Replace("-", "").Trim('0');
            if (string.IsNullOrWhiteSpace(s))
            {
                return "0";
            }

            if(isNegative)
            {
                s = "-" + s;
            }

            return s;
        }
    }
}
