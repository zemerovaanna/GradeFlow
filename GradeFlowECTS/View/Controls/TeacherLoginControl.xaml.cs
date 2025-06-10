using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GradeFlowECTS.Helpers;

namespace GradeFlowECTS.View.Controls
{
    public partial class TeacherLoginControl : UserControl
    {
        public TeacherLoginControl()
        {
            InitializeComponent();
        }

        private void Space_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Mail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InputValidator.IsValidMailCharacter(e.Text[0]);
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            ToggleVisibilityHelper(sender as Button, userPassword, userPasswordInput);
        }

        private void ToggleVisibilityHelper(Button? button, PasswordBox passwordBox, TextBox textBox)
        {
            if (passwordBox == null || textBox == null) return;

            if (passwordBox.Visibility == Visibility.Visible)
            {
                textBox.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Collapsed;
                textBox.Visibility = Visibility.Visible;
                if (button != null) button.Content = "🙂";
                textBox.Focus();
                textBox.CaretIndex = textBox.Text.Length;
            }
            else
            {
                passwordBox.Password = textBox.Text;
                passwordBox.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Collapsed;
                if (button != null) button.Content = "😎";
                passwordBox.Focus();
            }
        }
    }
}