using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GradeFlowECTS.Infrastructure
{
    public static class PasswordBoxService
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxService),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundPasswordChanged,
                    null,
                    false,
                    UpdateSourceTrigger.PropertyChanged));

        private static bool _isUpdating = false;

        public static string GetBoundPassword(DependencyObject d)
        {
            if (d == null)
            {
                return string.Empty;
            }

            var value = (string)d.GetValue(BoundPasswordProperty);
            return value;
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            if (d == null)
            {
                return;
            }

            d.SetValue(BoundPasswordProperty, value ?? string.Empty);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (!_isUpdating)
                {
                    passwordBox.Password = e.NewValue as string ?? string.Empty;
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _isUpdating = true;
                try
                {
                    SetBoundPassword(passwordBox, passwordBox.Password ?? string.Empty);
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }
    }
}