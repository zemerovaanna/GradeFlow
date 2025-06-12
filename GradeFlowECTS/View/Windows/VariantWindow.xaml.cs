using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.Trash;

namespace GradeFlowECTS.View.Windows
{
    public partial class VariantWindow : Window
    {
        public VariantWindow(Exam exam)
        {
            InitializeComponent();
            DataContext = new ExamViewModel(exam.ExamId);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}