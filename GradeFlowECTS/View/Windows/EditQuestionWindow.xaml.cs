using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class EditQuestionWindow : Window
    {
        private readonly ExamRepository _examRepository;
        private ObservableCollection<QuestionAnswerViewModel> _answers = new();
        private readonly Exam _exam;
        private readonly ExamTest _examTest;
        private readonly Question _editingQuestion;

        public EditQuestionWindow(Exam exam, ExamTest examTest, Question questionToEdit, ExamRepository examRepository)
        {
            InitializeComponent();
            _exam = exam;
            _examTest = examTest;
            _examRepository = examRepository;
            _editingQuestion = questionToEdit;

            AnswersPanel.ItemsSource = _answers;
            Load();
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

        private void Load()
        {
            var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();

            // Типы вопросов
            var types = _examRepository.GetQuestionTypes();
            QuestionTypeComboBox.ItemsSource = types;
            QuestionTypeComboBox.SelectedItem = QuestionTypeComboBox.Items
                .Cast<QuestionType>()
                .FirstOrDefault(q => q.QuestionTypeId == _editingQuestion.QuestionTypeId);

            // Темы
            var topics = _examRepository.GetTopicsWithQuestionsByDisciplineId(_exam.DisciplineId)
                .Select(t =>
                {
                    t.TopicName = LOL.Decrypt(t.TopicName);
                    return t;
                }).ToList();
            TopicsComboBox.ItemsSource = topics;
            TopicsComboBox.SelectedItem = TopicsComboBox.Items
                .Cast<TopicsDiscipline>()
                .FirstOrDefault(t => t.TopicDisciplinesId == _editingQuestion.TopicId);

            // Текст вопроса
            QuestionTextBox.Text = _editingQuestion.QuestionText;

            // Картинка вопроса
            if (_editingQuestion.FileData != null)
            {
                QuestionImage.Source = ByteArrayToImage(_editingQuestion.FileData);
                RemoveImageButton.Visibility = Visibility.Visible;
            }

            // Ответы
            foreach (var answer in _editingQuestion.QuestionAnswers.Select(q => { q.QuestionAnswerText = LOL.Decrypt(q.QuestionAnswerText); return q; }))
            {
                _answers.Add(new QuestionAnswerViewModel(answer));
            }

            if (_answers.Count == 0)
            {
                var model = new QuestionAnswer { QuestionAnswerText = "Ответ", IsCorrect = true };
                _answers.Add(new QuestionAnswerViewModel(model));
            }
        }

        private ImageSource ByteArrayToImage(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            // Обновление свойств вопроса
            _editingQuestion.QuestionTypeId = ((QuestionType)QuestionTypeComboBox.SelectedItem).QuestionTypeId;
            _editingQuestion.TopicId = ((TopicsDiscipline)TopicsComboBox.SelectedItem).TopicDisciplinesId;
            _editingQuestion.QuestionText = QuestionTextBox.Text;
            _editingQuestion.FileData = ConvertImageToBytes(QuestionImage.Source);
            _editingQuestion.Topic.TopicName = LOL.Encrypt(_editingQuestion.Topic.TopicName);

            // Обновление ответов
            _editingQuestion.QuestionAnswers.Clear();
            foreach (var answerVM in _answers)
            {
                answerVM.AnswerText = LOL.Encrypt(answerVM.AnswerText);
                _editingQuestion.QuestionAnswers.Add(answerVM.ToModel());
            }

            foreach (var q in _editingQuestion.Topic.Questions)
            {
                q.QuestionText = LOL.Encrypt(q.QuestionText);
                foreach (var a in q.QuestionAnswers)
                {
                    a.QuestionAnswerText = LOL.Encrypt(a.QuestionAnswerText);
                }
            }

            // Сохранение
            _examRepository.UpdateQuestion(_editingQuestion);
            DialogResult = true;
        }

        private byte[]? ConvertImageToBytes(ImageSource source)
        {
            if (source is BitmapImage bitmapImage)
            {
                using var ms = new MemoryStream();
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                return ms.ToArray();
            }
            return null;
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
                _editingQuestion.FileName = Path.GetFileName(openFileDialog.FileName);
                _editingQuestion.FileData = File.ReadAllBytes(openFileDialog.FileName);

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
            _editingQuestion.FileData = null;
            _editingQuestion.FileName = null;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}