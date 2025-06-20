using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class PasswordResetWindow : Window
    {
        User? currentUser;
        public PasswordResetWindow(string _mail)
        {
            InitializeComponent();
            var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            currentUser = context.Users.Where(u => u.Mail == _mail).FirstOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            errorText.Text = string.Empty;

            string newPass = newPasswordInput.Visibility == Visibility.Visible ?
                newPasswordInput.Text : newPassword.Password;
            string confirmPass = confirmPasswordInput.Visibility == Visibility.Visible ?
                confirmPasswordInput.Text : confirmPassword.Password;

            if (string.IsNullOrWhiteSpace(newPass))
            {
                errorText.Text = "Пароль не может быть пустым!";
                return;
            }

            if (!InputValidator.IsValidPassword(newPass, out string errorPassword))
            {
                errorText.Text = errorPassword;
                return;
            }

            if (newPass != confirmPass)
            {
                errorText.Text = "Пароли не совпадают.";
                return;
            }

            try
            {
                using (var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>())
                {
                    // Основная проблема была здесь - мы пытались использовать currentUser.Mail до проверки на null
                    // И не нужно делать новый запрос, так как currentUser уже получен в конструкторе

                    if (currentUser == null)
                    {
                        errorText.Text = "Пользователь не найден!";
                        return;
                    }

                    // Получаем пользователя из контекста для обновления
                    var userToUpdate = context.Users.Find(currentUser.UserId);

                    if (userToUpdate != null)
                    {
                        userToUpdate.Password = PasswordHasher.HashPassword(newPass);
                        context.SaveChanges();

                        newPassword.Password = string.Empty;
                        newPasswordInput.Text = string.Empty;
                        confirmPassword.Password = string.Empty;
                        confirmPasswordInput.Text = string.Empty;

                        errorText.Foreground = Brushes.Green;
                        errorText.Text = "Пароль успешно изменен!";
                    }
                    else
                    {
                        errorText.Text = "Пользователь не найден в базе данных!";
                    }
                }
            }
            catch (Exception ex)
            {
                errorText.Text = $"Ошибка при изменении пароля: {ex.Message}";
                // Для отладки можно добавить:
                // MessageBox.Show(ex.ToString());
            }
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag?.ToString() == "new")
            {
                ToggleVisibilityHelper(button, newPassword, newPasswordInput);
            }
            else if (button?.Tag?.ToString() == "confirm")
            {
                ToggleVisibilityHelper(button, confirmPassword, confirmPasswordInput);
            }
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