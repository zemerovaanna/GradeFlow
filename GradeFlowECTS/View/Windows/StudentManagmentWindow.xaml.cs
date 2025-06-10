using System.Windows;
using GradeFlowECTS.StudentManagment;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentManagmentWindow : Window
    {
        public StudentManagmentWindow()
        {
            InitializeComponent();
            var vm = new StudentsListViewModel();
            this.DataContext = vm;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}