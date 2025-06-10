using System.Windows;
using System.Windows.Controls;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentTestWindow : Window
    {
        public StudentTestWindow(ExamRepository examRepository)
        {
            InitializeComponent();
            DataContext = new TeacherTestViewModel(examRepository);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && DataContext is TeacherTestViewModel vm)
            {
                var answerId = (int)cb.Tag;
                var questionId = vm.CurrentQuestion?.QuestionId ?? 0;
                var ua = vm.UserAnswers.FirstOrDefault(x => x.QuestionId == questionId);
                if (ua != null && !ua.SelectedAnswerIds.Contains(answerId))
                    ua.SelectedAnswerIds.Add(answerId);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && DataContext is TeacherTestViewModel vm)
            {
                var answerId = (int)cb.Tag;
                var questionId = vm.CurrentQuestion?.QuestionId ?? 0;
                var ua = vm.UserAnswers.FirstOrDefault(x => x.QuestionId == questionId);
                if (ua != null && ua.SelectedAnswerIds.Contains(answerId))
                    ua.SelectedAnswerIds.Remove(answerId);
            }
        }
    }
}