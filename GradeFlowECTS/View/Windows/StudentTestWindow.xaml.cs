using System;
using System.Windows;
using System.Windows.Controls;
using GradeFlowECTS.Analyzers;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentTestWindow : Window
    {
        TeacherTestViewModel _vm;
        Exam _exam;

        public StudentTestWindow(ExamRepository examRepository, Exam exam)
        {
            InitializeComponent();

            _exam = exam;
            _vm = new TeacherTestViewModel(examRepository);
            DataContext = _vm;
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

        /*        private void FinishButton_Click(object sender, RoutedEventArgs e)
                {
                    _vm.FinishTest();
                    var result = _vm.ReturnResult();
                    string resulttxt =
                        $"📊 Процент: {result.Percent}%\n" +
                        $"🎯 Баллы: {result.Score}\n" +
                        $"🏅 Оценка: {result.Mark}\n" +
                        $"⏱ Время прохождения: {result.TimeSpent}";
                    var context = new GradeFlowContext();
                    var user = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
                    var student = context.Students.Where(s => s.StudentId == user.CurrentUser.StudentId).FirstOrDefault();
                    if (student != null)
                    {
                        DateTime now = DateTime.Now;
                        TimeOnly currentTime = TimeOnly.FromDateTime(now);
                        DateOnly currentDate = DateOnly.FromDateTime(now);

                        StudentExamResult studentExamResult = new StudentExamResult
                        {
                            StudentId = student.StudentId,
                            ExamId = _exam.ExamId,
                            TimeEnded = currentTime,
                            DateEnded = currentDate,
                            TotalScore = LOL.Encrypt(result.Score),
                            TestCriteria = resulttxt
                        };
                        context.StudentExamResults.Add(studentExamResult);
                        context.SaveChanges();
                        MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }*/

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.FinishTest();
            var result = _vm.ReturnResult();

            string resulttxt =
                $"📊 Процент: {result.Percent}%\n" +
                $"🎯 Баллы: {result.Score}\n" +
                $"🏅 Оценка: {result.Mark}\n" +
                $"⏱ Время прохождения: {result.TimeSpent}";

            var context = new GradeFlowContext();
            var user = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
            var student = context.Students.FirstOrDefault(s => s.StudentId == user.CurrentUser.StudentId);

            if (student != null)
            {
                DateTime now = DateTime.Now;
                TimeOnly currentTime = TimeOnly.FromDateTime(now);
                DateOnly currentDate = DateOnly.FromDateTime(now);

                // Поиск существующего результата
                Guid examId = _exam.ExamId;

                var existingResult = context.StudentExamResults
                    .FirstOrDefault(r => r.StudentId == student.StudentId && r.ExamId == examId);

                if (existingResult != null)
                {
                    // Обновление существующего результата
                    existingResult.TimeEnded = currentTime;
                    existingResult.DateEnded = currentDate;
                    existingResult.TotalScore = LOL.Encrypt(result.Score);
                    existingResult.TestCriteria = LOL.Encrypt(resulttxt);
                    existingResult.TestTimeSpent = LOL.Encrypt(result.TimeSpent.ToString());

                    context.StudentExamResults.Update(existingResult);
                }
                else
                {
                    // Создание нового результата
                    StudentExamResult newResult = new StudentExamResult
                    {
                        StudentId = student.StudentId,
                        ExamId = _exam.ExamId,
                        TimeEnded = currentTime,
                        DateEnded = currentDate,
                        TotalScore = LOL.Encrypt(result.Score),
                        TestCriteria = LOL.Encrypt(resulttxt),
                        TestTimeSpent = LOL.Encrypt(result.TimeSpent.ToString())
                    };

                    context.StudentExamResults.Add(newResult);
                }

                context.SaveChanges();

                MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}