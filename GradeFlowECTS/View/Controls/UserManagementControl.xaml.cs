using System.Windows.Controls;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Controls
{
    public partial class UserManagementControl : UserControl
    {
        private readonly UserManagementViewModel _userManagementViewModel;
        public UserManagementControl()
        {
            InitializeComponent();

            _userManagementViewModel = App.Current.ServiceProvider.GetRequiredService<UserManagementViewModel>();

            _userManagementViewModel.GoExistingMailsAction = mails =>
            {
                ExistingMailsWindow window = new ExistingMailsWindow(mails);
                window.ShowDialog();
            };

            DataContext = _userManagementViewModel;
        }

        private void StudentManagmentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudentManagmentWindow window = new StudentManagmentWindow();
            if(window.ShowDialog() == true)
            {
                _userManagementViewModel.ApplyFilter();
            }
        }
    }
}