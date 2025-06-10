using System.Windows;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class InterludeWindow : Window
    {
        public InterludeWindow(InterludeViewModel interludeViewModel)
        {
            InitializeComponent();
            DataContext = interludeViewModel;
            interludeViewModel.CloseRequested += (s, e) => Close();
        }
    }
}