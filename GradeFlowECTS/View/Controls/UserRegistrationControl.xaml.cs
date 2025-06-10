using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Controls
{
    public partial class UserRegistrationControl : UserControl
    {
        private readonly UserRegistrationViewModel _userRegistrationViewModel;

        public UserRegistrationControl()
        {
            InitializeComponent();
            _userRegistrationViewModel = App.Current.ServiceProvider.GetRequiredService<UserRegistrationViewModel>();
            DataContext = _userRegistrationViewModel;
            _userRegistrationViewModel.PasswordGenerated += ViewModel_PasswordGenerated;
        }

        private void ViewModel_PasswordGenerated(string generatedPassword)
        {
            userPasswordBox.Focus();
            if (userPasswordBox.Visibility == Visibility.Collapsed)
            {
                userPasswordInput.Focus();
                userPasswordInput.CaretIndex = userPasswordInput.Text.Length;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_userRegistrationViewModel != null)
            {
                _userRegistrationViewModel.PasswordGenerated -= ViewModel_PasswordGenerated;
            }
        }

        private void Space_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[А-Яа-я'-]+$"))
            {
                e.Handled = true;
            }
        }

        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string validCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._-@";
            if (!string.IsNullOrEmpty(e.Text) && !validCharacters.Contains(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void VerificationCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string validCharacters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789";
            if (!string.IsNullOrEmpty(e.Text) && !e.Text.ToUpper().All(c => validCharacters.Contains(c)))
            {
                e.Handled = true;
            }
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            ToggleVisibilityHelper(sender as Button, userPasswordBox, userPasswordInput);
            if (userPasswordBox.Visibility == Visibility.Visible)
            {
                userPasswordBox.Password = userPasswordInput.Text;
            }
            else
            {
                userPasswordInput.Text = userPasswordBox.Password;
            }
        }

        private void ToggleConfirmPasswordVisibility(object sender, RoutedEventArgs e)
        {
            ToggleVisibilityHelper(sender as Button, confirmPasswordBox, confirmPasswordInput);

            if (confirmPasswordBox.Visibility == Visibility.Visible)
            {
                confirmPasswordBox.Password = confirmPasswordInput.Text;
            }
            else
            {
                confirmPasswordInput.Text = confirmPasswordBox.Password;
            }
        }

        private void ToggleVisibilityHelper(Button? btn, PasswordBox pBox, TextBox tBox)
        {
            if (pBox == null || tBox == null) return;

            if (pBox.Visibility == Visibility.Visible)
            {
                pBox.Visibility = Visibility.Collapsed;
                tBox.Visibility = Visibility.Visible;
                if (btn != null) btn.Content = "🙂";
                tBox.Focus();
                tBox.CaretIndex = tBox.Text.Length;
            }
            else
            {
                pBox.Visibility = Visibility.Visible;
                tBox.Visibility = Visibility.Collapsed;
                if (btn != null) btn.Content = "😎";
                pBox.Focus();
            }
        }
    }
}