using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class TestResultWindow : Window
    {
        public TestResultWindow(Exam exam, ExamRepository examRepository)
        {
            InitializeComponent();
            DataContext = new TestResultsViewModel(examRepository, exam);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}