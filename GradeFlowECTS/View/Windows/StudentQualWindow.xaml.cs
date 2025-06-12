using System.Windows;
using GradeFlowECTS.Analyzers;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentQualWindow : Window
    {
        AnalyzerViewModel _vm;
        public StudentQualWindow(int studentId, Guid examId)
        {
            InitializeComponent();
            _vm = new AnalyzerViewModel(studentId, examId);
            DataContext = _vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AnalyzerViewModel vm)
            {
                vm.AnalyzeFiles();
                var studentExamResult = _vm.ReturnResult();
                GradeFlowContext context = new GradeFlowContext();
                context.StudentExamResults.Add(studentExamResult);
                context.SaveChanges();
                MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}