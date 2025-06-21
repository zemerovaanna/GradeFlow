using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using GradeFlowECTS.Analyzers;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentQualWindow : Window
    {
        AnalyzerViewModel _vm;
        Variant _variant;
        private readonly int? _windowCloseTimeoutMinutes;
        private TimeSpan _remainingTime;
        private readonly DispatcherTimer _countdownTimer;
        private int? было;
        private int _STUDENT_ID;
        private Guid _EXAM_ID;

        public StudentQualWindow(int studentId, Guid examId)
        {
            InitializeComponent();

            _STUDENT_ID = studentId;
            _EXAM_ID = examId;
            _vm = new AnalyzerViewModel(studentId, examId);
            DataContext = _vm;

            var context = new GradeFlowContext();
            var temp = context.StudentExamResults.Where(r => r.StudentId == studentId && r.ExamId == examId).FirstOrDefault();
            if (temp != null)
            {
                _variant = context.Variants.Where(v => v.VariantNumber == temp.VariantNumber).FirstOrDefault();
                TaskText.Text = LOL.Decrypt(_variant.VariantText);
            }

            _windowCloseTimeoutMinutes = context.ExamPractices.Where(e => e.ExamId == examId).Select(e => e.PracticeTimeToComplete).FirstOrDefault();
            было = _windowCloseTimeoutMinutes;

            var existingResult = context.StudentExamResults.FirstOrDefault(r =>
                r.StudentId == studentId &&
                r.ExamId == examId);

            if (existingResult != null)
            {
                // Обновляем существующий результат
                existingResult.VariantNumber = _variant.VariantNumber;
                existingResult.PracticeTimeSpent = LOL.Encrypt("00:00");

                context.StudentExamResults.Update(existingResult);
            }
            else
            {
                // Добавляем новый результат
                var studentExamResult = new StudentExamResult
                {
                    StudentId = studentId,
                    ExamId = examId,
                    VariantNumber = _variant.VariantNumber,
                    PracticeTimeSpent = LOL.Encrypt("00:00")
                };

                context.StudentExamResults.Add(studentExamResult);
            }

            context.SaveChanges();

            if (_windowCloseTimeoutMinutes != 0 && _windowCloseTimeoutMinutes != null)
            {
                _remainingTime = TimeSpan.FromMinutes(Convert.ToDouble(_windowCloseTimeoutMinutes));

                _countdownTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _countdownTimer.Tick += CountdownTimer_Tick;
                _countdownTimer.Start();

                UpdateCountdownDisplay();
            }
            else
            {
                CountdownLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateCountdownDisplay();

            if (_remainingTime <= TimeSpan.Zero)
            {
                _countdownTimer.Stop();

                DateTime now = DateTime.Now;
                TimeOnly currentTime = TimeOnly.FromDateTime(now);
                DateOnly currentDate = DateOnly.FromDateTime(now);

                var context = new GradeFlowContext();

                var existingResult = context.StudentExamResults.FirstOrDefault(r =>
                    r.StudentId == _STUDENT_ID &&
                    r.ExamId == _EXAM_ID);

                TimeSpan elapsed = TimeSpan.FromMinutes(Convert.ToDouble(_windowCloseTimeoutMinutes)) - _remainingTime;
                string timeResult = elapsed.ToString(@"mm\:ss") + $"/{было}:00";

                if (existingResult != null)
                {
                    // Обновляем существующий результат
                    existingResult.TimeEnded = currentTime;
                    existingResult.DateEnded = currentDate;
                    existingResult.VariantNumber = _variant.VariantNumber;
                    existingResult.PracticeTimeSpent = LOL.Encrypt(timeResult);

                    context.StudentExamResults.Update(existingResult);
                }
                else
                {
                    // Добавляем новый результат
                    var studentExamResult = new StudentExamResult
                    {
                        StudentId = _STUDENT_ID,
                        ExamId = _EXAM_ID,
                        TimeEnded = currentTime,
                        DateEnded = currentDate,
                        VariantNumber = _variant.VariantNumber,
                        PracticeTimeSpent = LOL.Encrypt(timeResult)
                    };

                    context.StudentExamResults.Add(studentExamResult);
                }

                context.SaveChanges();




                Close(); // или Application.Current.Shutdown(), если ты хочешь всё завершить
            }
        }

        private void UpdateCountdownDisplay()
        {
            CountdownLabel.Content = _remainingTime.ToString(@"mm\:ss");
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

        /*        private void Button_Click(object sender, RoutedEventArgs e)
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
                }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AnalyzerViewModel vm)
            {
                vm.AnalyzeFiles();
                var result = _vm.ReturnResult();
                result.VariantNumber = _variant.VariantNumber;

                var context = new GradeFlowContext();
                var user = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
                var studentId = user.CurrentUser.StudentId;
                var examId = result.ExamId;

                var existingResult = context.StudentExamResults.FirstOrDefault(r => r.StudentId == studentId && r.ExamId == examId);
                
                TimeSpan elapsed = TimeSpan.FromMinutes(Convert.ToDouble(_windowCloseTimeoutMinutes)) - _remainingTime;
                string timeResult = elapsed.ToString(@"mm\:ss") + $"/{было}:00";

                if (existingResult != null)
                {
                    // Обновление существующего результата
                    existingResult.TimeEnded = result.TimeEnded;
                    existingResult.DateEnded = result.DateEnded;
                    existingResult.QualCriteria = result.QualCriteria;
                    existingResult.PracticeTotalScore = result.PracticeTotalScore;
                    existingResult.VariantNumber = result.VariantNumber;
                    existingResult.PracticeTimeSpent = LOL.Encrypt(timeResult);

                    context.StudentExamResults.Update(existingResult);
                }
                else
                {
                    // Добавление нового результата
                    result.PracticeTimeSpent = LOL.Encrypt(timeResult);
                    context.StudentExamResults.Add(result);
                }

                context.SaveChanges();

                MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}