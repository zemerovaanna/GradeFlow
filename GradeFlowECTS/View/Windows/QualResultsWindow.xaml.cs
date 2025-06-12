using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class QualResultsWindow : Window
    {
        public QualResultsWindow(Exam exam, ExamRepository examRepository)
        {
            InitializeComponent();
            DataContext = new QualResultsViewModel(examRepository, exam);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}