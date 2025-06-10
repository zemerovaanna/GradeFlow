using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class TestManagmentViewModel : BaseViewModel
    {
        private readonly IExamRepository _examRepository;
        private readonly Exam _exam;
        private readonly ExamTest _examTest;

        public Visibility IsTopicsEmpty => Topics.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        private readonly bool _isNewTest;

        public ICommand RemoveQuestionCommand { get; }
        public ICommand EditQuestionCommand { get; }

        public TestManagmentViewModel(IExamRepository examRepository, Exam exam, ExamTest examTest, bool isNewTest)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _exam = exam ?? throw new ArgumentNullException(nameof(exam));
            _examTest = examTest ?? throw new ArgumentNullException(nameof(examTest));
            _isNewTest = isNewTest;

            RecalculateTotalPoints();
            Minutes = _examTest.TimeToComplete;

            LoadTopicsAndQuestions();

            RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
            EditQuestionCommand = new RelayCommand(EditQuestion);
        }

        private void EditQuestion(object? parameter)
        {
            if (parameter is Question question)
            {
                var window = new EditQuestionWindow(_exam, _examTest, question, new ExamRepository());
                if(window.ShowDialog() == true)
                {
                    LoadTopicsAndQuestions();
                    RecalculateTotalPoints();
                }
            }
        }

        private void RemoveQuestion(object? parameter)
        {
            if (parameter is Question question)
            {
                var result = MessageBox.Show(
                    "Вы действительно хотите удалить этот вопрос и все его ответы?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _examRepository.RemoveQuestion(question.QuestionId);
                    LoadTopicsAndQuestions();
                }
                RecalculateTotalPoints();
            }
        }

        public void RecalculateTotalPoints()
        {
            TotalPoints = (byte)_examRepository.RecalculateTotalPoints(_examTest.ExamTestId);
            OnPropertyChanged(nameof(TotalPoints));
        }

        public byte TotalPoints
        {
            get => _examTest.TotalPoints;
            set
            {
                _examTest.TotalPoints = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int Minutes
        {
            get => _examTest.TimeToComplete;
            set
            {
                if (value < 0 || value > 99)
                {
                    TimeBorderBrush = Brushes.Red;
                }
                else
                {
                    _examTest.TimeToComplete = value;
                    TimeBorderBrush = Brushes.Gray;
                }
                OnPropertyChanged(nameof(Minutes));
            }
        }

        private Brush _timeBorderBrush = Brushes.Gray;
        public Brush TimeBorderBrush
        {
            get => _timeBorderBrush;
            set
            {
                if (_timeBorderBrush != value)
                {
                    _timeBorderBrush = value;
                    OnPropertyChanged(nameof(TimeBorderBrush));
                }
            }
        }

        private ObservableCollection<TopicWithQuestionsViewModel> _topics = new();
        public ObservableCollection<TopicWithQuestionsViewModel> Topics
        {
            get => _topics;
            set
            {
                _topics = value;
                OnPropertyChanged(nameof(Topics));
            }
        }

        public void LoadTopicsAndQuestions()
        {
            var disciplineId = _exam.DisciplineId;

            HashSet<int> selectedTopics;
            if (_isNewTest)
            {
                selectedTopics = _examRepository
                    .GetTopicsWithQuestionsByDisciplineId(disciplineId)
                    .Select(t => t.TopicDisciplinesId)
                    .ToHashSet();
            }
            else
            {
                selectedTopics = _examRepository
                    .GetSelectedTopicsByTestId(_examTest.ExamTestId)
                    .Select(t => t.TopicId)
                    .ToHashSet();
            }

            var decryptedTopics = _examRepository.GetTopicsWithQuestionsByDisciplineId(disciplineId)
                .Where(t => t.DisciplineId == disciplineId)
                .Select(t =>
                {
                    t.TopicName = LOL.Decrypt(t.TopicName);
                    foreach (var q in t.Questions)
                    {
                        q.QuestionText = LOL.Decrypt(q.QuestionText);
                    }
                    return t;
                });

            Topics = new ObservableCollection<TopicWithQuestionsViewModel>(
                decryptedTopics.Select(t => new TopicWithQuestionsViewModel(
                    t,
                    t.Questions.ToList(),
                    selectedTopics.Contains(t.TopicDisciplinesId)
                ))
            );

        }

        static class LOL
        {
            private const int KeySize = 32;
            private const int IvSize = 12;
            private const int TagSize = 16;

            public static string Encrypt(string? plainText)
            {
                try
                {
                    if (plainText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);
                        byte[] iv = new byte[IvSize];

                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(iv);
                        }

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            byte[] cipherBytes = new byte[plainBytes.Length];
                            byte[] tag = new byte[TagSize];

                            aes.Encrypt(iv, plainBytes, cipherBytes, tag);

                            byte[] encryptedData = new byte[IvSize + cipherBytes.Length + TagSize];
                            Array.Copy(iv, 0, encryptedData, 0, IvSize);
                            Array.Copy(cipherBytes, 0, encryptedData, IvSize, cipherBytes.Length);
                            Array.Copy(tag, 0, encryptedData, IvSize + cipherBytes.Length, TagSize);

                            return Convert.ToBase64String(encryptedData);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static string Decrypt(string? cipherText)
            {
                try
                {
                    if (cipherText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);

                        cipherText = cipherText.PadRight(cipherText.Length + (4 - cipherText.Length % 4) % 4, '=');
                        byte[] cipherData = Convert.FromBase64String(cipherText);

                        byte[] iv = new byte[IvSize];
                        byte[] tag = new byte[TagSize];
                        byte[] cipherBytes = new byte[cipherData.Length - IvSize - TagSize];

                        Array.Copy(cipherData, 0, iv, 0, IvSize);
                        Array.Copy(cipherData, IvSize, cipherBytes, 0, cipherBytes.Length);
                        Array.Copy(cipherData, IvSize + cipherBytes.Length, tag, 0, TagSize);

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = new byte[cipherBytes.Length];
                            aes.Decrypt(iv, cipherBytes, tag, plainBytes);
                            return Encoding.UTF8.GetString(plainBytes);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            private static byte[] GetKey(string key)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }

            private static string ComplexComputation()
            {
                int[] values = { 1012, 3, 5, 7, 4 };
                int sum = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    sum += values[i] * (i % 2 == 0 ? 2 : 3);
                }
                sum -= Fibonacci(5) * 10;
                sum += Factorial(3);
                return $"{sum}ects2025";
            }

            private static int Fibonacci(int n)
            {
                if (n <= 1) return n;
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }

            private static int Factorial(int n)
            {
                if (n <= 1) return 1;
                return n * Factorial(n - 1);
            }
        }

        public bool IsValid()
        {
            return _examTest.TimeToComplete >= 0 && _examTest.TimeToComplete <= 99;
        }

        public void SaveExamTest()
        {
            if (IsValid())
            { 
                List<Question> questions = new List<Question>();
                foreach (TopicWithQuestionsViewModel topic in Topics)
                {
                    questions.AddRange(topic.Questions);
                }

                RecalculateTotalPoints();
                _examRepository.UpdateExamTest(_examTest);
                _examRepository.SaveSelectedTopics(_examTest.ExamTestId, Topics.ToList());
                _examRepository.SaveSelectedQuestions(_examTest.ExamTestId, questions);
            }
            else
            {
                MessageBox.Show("Время на тест должно быть от 0 до 99 минут.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}