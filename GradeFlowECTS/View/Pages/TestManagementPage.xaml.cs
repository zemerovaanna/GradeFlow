using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Pages
{
    public partial class TestManagementPage : Page
    {
        private readonly TestManagmentViewModel _testManagmentViewModel;
        private readonly ExamRepository _examRepository;
        private readonly Exam _exam;
        private ExamTest _examTest;

        public TestManagementPage(ExamRepository examRepository)
        {
            InitializeComponent();

            IExamContext examContext = App.Current.ServiceProvider.GetRequiredService<IExamContext>();
            _examRepository = examRepository;
            _exam = _examRepository.GetExamById(examContext.CurrentExamId);
            var test = _examRepository.GetExamTestByExamId(_exam.ExamId);
            bool isNewTest = test == null;
            _examTest = isNewTest ? _examRepository.CreateExamTest(_exam.ExamId) : test;
            _testManagmentViewModel = new TestManagmentViewModel(_examRepository, _exam, _examTest, isNewTest);
            DataContext = _testManagmentViewModel;
        }

        private static readonly Regex NumberOnly = new Regex("^[0-9]+$");
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberOnly.IsMatch(e.Text);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _testManagmentViewModel.SaveExamTest();
            NavigationService?.GoBack();
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            _testManagmentViewModel.SaveExamTest();
            _testManagmentViewModel.RecalculateTotalPoints();

            if (_testManagmentViewModel.TotalPoints >= 90)
            {
                MessageBox.Show(
                    "Нельзя добавить больше вопросов — тест почти достиг максимального количества баллов (100).",
                    "Ограничение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            AddQuestionWindow addQuestionWindow = new AddQuestionWindow(_exam, _examTest, _examRepository);
            if (addQuestionWindow.ShowDialog() == true)
            {
                _testManagmentViewModel.LoadTopicsAndQuestions();
            }
        }

        private void TopicCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TopicWithQuestionsViewModel topic)
            {
                foreach (var question in topic.Questions) question.IsSelected = true;
                int index = _testManagmentViewModel.Topics.IndexOf(topic);
                _testManagmentViewModel.Topics.Remove(topic);
                _testManagmentViewModel.Topics.Insert(index, topic);
                _testManagmentViewModel.SaveExamTest();
                _testManagmentViewModel.RecalculateTotalPoints();
            }
        }

        private void TopicCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TopicWithQuestionsViewModel topic)
            {
                int err = 0;
                if (topic.Questions.Any(q => q.IsSelected == false)) err++;
                if (err == 0)
                {
                    foreach (var question in topic.Questions) question.IsSelected = false;
                    int index = _testManagmentViewModel.Topics.IndexOf(topic);
                    _testManagmentViewModel.Topics.Remove(topic);
                    _testManagmentViewModel.Topics.Insert(index, topic);
 
                }
                _testManagmentViewModel.SaveExamTest();
                _testManagmentViewModel.RecalculateTotalPoints();
            }
        }

        private void QuestionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Question question)
            {
                var topic = _testManagmentViewModel.Topics.First(t => t.Questions.Contains(question));
                int err = 0;
                if (topic.Questions.Any(q => q.IsSelected == false)) err++;
                if (err == 0)
                {
                    topic.IsSelected = true;
                }
                _testManagmentViewModel.SaveExamTest();
                _testManagmentViewModel.RecalculateTotalPoints();
            }
        }
        private void QuestionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Question question)
            {
                var topic = _testManagmentViewModel.Topics.First(t => t.Questions.Contains(question));
                topic.IsSelected = false;
                _testManagmentViewModel.SaveExamTest();
                _testManagmentViewModel.RecalculateTotalPoints();
            }
        }
    }
}