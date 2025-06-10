using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class AddQuestionWindow : Window
    {
        private readonly ExamRepository _examRepository;

        private Question _newQuestion = new();
        private ObservableCollection<QuestionAnswerViewModel> _answers = new();

        private Exam _exam;
        private ExamTest _examTest;

        public AddQuestionWindow(Exam exam, ExamTest examTest, ExamRepository examRepository)
        {
            InitializeComponent();

            _examRepository = examRepository;
            _exam = exam;
            _examTest = examTest;

            AnswersPanel.ItemsSource = _answers;
            Load();
        }

        private void Load()
        {
            var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();

            var types = _examRepository.GetQuestionTypes();
            QuestionTypeComboBox.ItemsSource = types;
            if (QuestionTypeComboBox.Items.Count > 0)
            {
                QuestionTypeComboBox.SelectedIndex = 0;
            }

            AesGcmCryptographyService cryptographyService = new AesGcmCryptographyService();
            FileService fileService = new FileService("GradeFlow");
            UserSettingsService userSettingsService = new UserSettingsService(fileService);
            ConfigurationService configurationService = new ConfigurationService(cryptographyService, fileService, userSettingsService);
            var topicRepository = new TopicRepository(context, cryptographyService, configurationService);
            var topics = _examRepository.GetTopicsWithQuestionsByDisciplineId(_exam.DisciplineId)
                .Select(t =>
                {
                    t.TopicName = LOL.Decrypt(t.TopicName);
                    return t;
                }).ToList();
            TopicsComboBox.ItemsSource = topics;
            if (TopicsComboBox.Items.Count > 0)
            {
                TopicsComboBox.SelectedIndex = 0;
            }

            var model = new QuestionAnswer { QuestionAnswerText = "Ответ", IsCorrect = true };
            _answers.Add(new QuestionAnswerViewModel(model));
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

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (_answers.Count >= 10)
            {
                MessageBox.Show("Нельзя добавить больше 10 вариантов ответа.", "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var model = new QuestionAnswer { QuestionAnswerText = "", IsCorrect = false };
            _answers.Add(new QuestionAnswerViewModel(model));

            UpdateAddAnswerButtonState();
        }

        private void UpdateAddAnswerButtonState()
        {
            AddAnswerButton.IsEnabled = _answers.Count < 10;
        }


        private void AnswerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedType = QuestionTypeComboBox.SelectedItem as QuestionType;

            if (selectedType?.QuestionTypeId == 1)
            {
                var current = (sender as CheckBox)?.DataContext as QuestionAnswerViewModel;
                foreach (var answer in _answers)
                {
                    if (answer != current)
                        answer.Correct = false;
                }
            }
        }

        private void AttachAnswerImage_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not QuestionAnswerViewModel answer) return;

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                answer.FileName = Path.GetFileName(openFileDialog.FileName);
                answer.FileData = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
        private void LoadQuestionImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _newQuestion.FileName = Path.GetFileName(openFileDialog.FileName);
                _newQuestion.FileData = File.ReadAllBytes(openFileDialog.FileName);

                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                QuestionImage.Source = bitmap;
                RemoveImageButton.Visibility = Visibility.Visible;
            }
        }

        private void QuestionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = QuestionTypeComboBox.SelectedItem as QuestionType;

            if (selectedType?.QuestionTypeId == 1)
            {
                bool found = false;
                foreach (var answer in _answers)
                {
                    if (answer.Correct)
                    {
                        if (!found)
                        {
                            found = true;
                        }
                        else
                        {
                            answer.Correct = false;
                        }
                    }
                }
            }
        }

        private void RemoveQuestionImage_Click(object sender, RoutedEventArgs e)
        {
            _newQuestion.FileData = null;
            _newQuestion.FileName = null;
            QuestionImage.Source = null;
            RemoveImageButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveAnswerImage_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not QuestionAnswerViewModel answer) return;

            answer.FileData = null;
            answer.FileName = null;
        }

        private void RemoveAnswer_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not QuestionAnswerViewModel answer) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить этот ответ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _answers.Remove(answer);
                UpdateAddAnswerButtonState();
            }
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            // 1. Текст вопроса.
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите текст вопроса.", "Ошибка");
                return;
            }

            // 2. Валидные ответы (текст или картинка).
            var validAnswers = _answers
                .Where(a => !string.IsNullOrWhiteSpace(a.AnswerText) || (a.FileData != null && a.FileData.Length > 0))
                .ToList();

            if (validAnswers.Count < 2)
            {
                MessageBox.Show("Должно быть как минимум два валидных ответа (с текстом или изображением).", "Ошибка");
                return;
            }

            // 3. Дубликаты текста.
            var duplicateTexts = validAnswers
                .Where(a => !string.IsNullOrWhiteSpace(a.AnswerText))
                .GroupBy(a => a.AnswerText.Trim())
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateTexts.Any())
            {
                MessageBox.Show("Есть повторяющиеся варианты ответов. Убедитесь, что все тексты уникальны.", "Ошибка");
                return;
            }

            // 4. Хоть один правильный.
            if (!validAnswers.Any(a => a.Correct))
            {
                MessageBox.Show("Среди валидных ответов должен быть хотя бы один правильный.", "Ошибка");
                return;
            }

            // Всё хорошо — сохраняем.
            _newQuestion.QuestionText = QuestionTextBox.Text;
            _newQuestion.QuestionAnswers = validAnswers.Select(vm => vm.Model).ToList();
            _newQuestion.QuestionTypeId = (QuestionTypeComboBox.SelectedItem as QuestionType)?.QuestionTypeId ?? 1;
            _newQuestion.TopicId = (TopicsComboBox.SelectedItem as TopicsDiscipline)?.TopicDisciplinesId ?? 1;

            bool result = _examRepository.AddQuestionToExam(_exam.ExamId, _newQuestion);

            if (result)
            {
                MessageBox.Show("Вопрос успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении вопроса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}