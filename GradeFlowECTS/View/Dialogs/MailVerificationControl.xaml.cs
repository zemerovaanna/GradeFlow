using System.Windows;
using System.Windows.Controls;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Dialogs
{
    public partial class MailVerificationControl : UserControl
    {
        private readonly MailVerificationViewModel _mailVerificationViewModel;

        public MailVerificationControl()
        {
            InitializeComponent();
            _mailVerificationViewModel = App.Current.ServiceProvider.GetRequiredService<MailVerificationViewModel>();

            codePassword.PasswordChanged += (sender, e) =>
            {
                _mailVerificationViewModel.Code = codePassword.Password;
            };

            DataContext = _mailVerificationViewModel;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window? window = Window.GetWindow(this);
            window?.Close();
        }
    }
}