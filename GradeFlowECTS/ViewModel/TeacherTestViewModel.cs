using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GradeFlowECTS.Core;
using GradeFlowECTS.Data;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.ViewModel
{
    public class TeacherTestViewModel : BaseViewModel
    {
        private readonly IExamRepository _repository;

        public TeacherTestViewModel(IExamRepository repository)
        {
            _repository = repository;
            LoadTest(App.Current.ServiceProvider.GetRequiredService<IExamContext>().CurrentExamId);

            NextCommand = new RelayCommand(_ => Next());
            PreviousCommand = new RelayCommand(_ => Previous());
            FinishCommand = new RelayCommand(_ => FinishTest());

            FirstQuestion = Visibility.Hidden;
            LastQuestion = Visibility.Visible;

            CurrentQuestionNumber = 1;
        }

        private Guid _examId;
        private ExamTest _examTest;
        private List<Question> _questions;
        private int _currentQuestionIndex = 0;
        private DispatcherTimer? _timer;
        private TimeSpan _timeRemaining;
        private DateTime _startTime;

        public Visibility FirstQuestion {  get; private set; }
        public Visibility LastQuestion { get; private set; }

        public int CurrentQuestionNumber { get; private set; }

        public Question? CurrentQuestion => _questions?.Count > 0 ? _questions[_currentQuestionIndex] : null;
        public ObservableCollection<UserAnswer> UserAnswers { get; } = new();

        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand FinishCommand { get; }

        public Action LOLOL { get; set; }

        public string TimeRemainingText => _timeRemaining.ToString(@"mm\:ss");

        private void LoadTest(Guid examId)
        {
            _startTime = DateTime.Now;
            _examId = examId;
            _examTest = _repository.GetExamTestByExamId(examId);
            /*var selectedTopicIds = _repository.GetSelectedTopicsByTestId(_examTest.ExamTestId)
                                              .Where(t => t.IsSelected)
                                              .Select(t => t.TopicId)
                                              .ToHashSet();*/


            _questions = _examTest.Questions
                .Where(q => q.IsSelected == true)
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            if (_examTest.Exam.Discipline.DisciplineName == "МДК 01.02")
            {
                _questions = _questions.Take(30).ToList();
            }

            if (_questions.Count == 0)
            {
                MessageBox.Show("Нет доступных вопросов для выбранных тем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var q in _questions)
            {
                // 🔓 Расшифровка текста вопроса
                q.QuestionText = _repository.Decrypt(q.QuestionText);

                // 🔄 Перемешать ответы
                q.QuestionAnswers = q.QuestionAnswers.OrderBy(_ => Guid.NewGuid()).ToList();

                // 🔓 Расшифровка текста каждого ответа
                foreach (var a in q.QuestionAnswers)
                {
                    a.QuestionAnswerText = _repository.Decrypt(a.QuestionAnswerText);
                }
            }

            foreach (var q in _questions)
                UserAnswers.Add(new UserAnswer { QuestionId = q.QuestionId });

            OnPropertyChanged(nameof(CurrentQuestion));

            if (_examTest.TimeToComplete > 0)
            {
                _timeRemaining = TimeSpan.FromMinutes(_examTest.TimeToComplete);
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _timer.Tick += (s, e) =>
                {
                    _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
                    OnPropertyChanged(nameof(TimeRemainingText));

                    if (_timeRemaining <= TimeSpan.Zero)
                    {
                        _timer.Stop();
                        FinishTest();
                    }
                };
                _timer.Start();
            }
        }

        public void Next() { if (_currentQuestionIndex < _questions.Count - 1) { _currentQuestionIndex++; OnPropertyChanged(nameof(CurrentQuestion)); CurrentQuestionNumber = _currentQuestionIndex + 1; OnPropertyChanged(nameof(CurrentQuestionNumber)); ShowHideButtons(); } }
        public void Previous() { if (_currentQuestionIndex > 0) { _currentQuestionIndex--; OnPropertyChanged(nameof(CurrentQuestion)); CurrentQuestionNumber = _currentQuestionIndex + 1; OnPropertyChanged(nameof(CurrentQuestionNumber)); ShowHideButtons(); } }

        public void FinishTest()
        {
            var timeSpent = DateTime.Now - _startTime;
            _timer.Stop();
            FirstQuestion = Visibility.Collapsed;
            LastQuestion = Visibility.Collapsed;
            OnPropertyChanged(nameof(FirstQuestion));
            OnPropertyChanged(nameof(LastQuestion));
            int score = 0;
            foreach (var q in _questions)
            {
                var correct = q.QuestionAnswers.Where(a => a.IsCorrect).Select(a => a.QuestionAnswerId).ToHashSet();
                var selected = UserAnswers.First(a => a.QuestionId == q.QuestionId).SelectedAnswerIds;

                if (correct.SetEquals(selected))
                    score += correct.Count;
            }

            int total = _examTest.TotalPoints;
            double percent = total == 0 ? 0 : (double)score / total * 100;

            string mark = percent switch
            {
                >= 90 => "5",
                >= 75 => "4",
                >= 60 => "3",
                _ => "2"
            };

            
            var context = new GradeFlowContext();
            var user = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
            var student = context.Students.Where(s => s.StudentId == user.CurrentUser.StudentId).FirstOrDefault();
            if (student != null)
            {
                studentId = student.StudentId;
                result = new Result
                {
                    Percent = Math.Round(percent,1).ToString(),
                    Score = $"{score}/{total}",
                    Mark = mark.ToString(),
                    TimeSpent = timeSpent.ToString(@"mm\:ss")
                };
            }
            else
            {
                MessageBox.Show($"{Math.Round(percent)} %\nБаллы: {score}/{total}\nОценка: {mark}", "Результат", MessageBoxButton.OK);
            }

            LOLOL?.Invoke();
        }



        int studentId;
        Result result;

        public Result ReturnResult()
        {
            return result;
        }

        private void ShowHideButtons()
        {
            FirstQuestion = GetVisibility(_currentQuestionIndex == 0);
            LastQuestion = GetVisibility(_currentQuestionIndex == _questions.Count - 1);

            OnPropertyChanged(nameof(FirstQuestion));
            OnPropertyChanged(nameof(LastQuestion));
        }

        private Visibility GetVisibility(bool value)
        {
            if (value) return Visibility.Hidden;
            else return Visibility.Visible;
        }
    }
}