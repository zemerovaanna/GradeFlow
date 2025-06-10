using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class MDK01ResultsWindow : Window
    {
        public MDK01ResultsWindow(Exam exam, ExamRepository examRepository)
        {
            InitializeComponent();
            DataContext = new MDK01ResultsViewModel(examRepository, exam);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}