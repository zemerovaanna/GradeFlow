using System.Security.Cryptography;
using System.Text;
using System.Windows;
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

        public StudentQualWindow(int studentId, Guid examId)
        {
            InitializeComponent();
            _vm = new AnalyzerViewModel(studentId, examId);
            DataContext = _vm;

            var context = new GradeFlowContext();
            var temp = context.StudentExamResults.Where(r => r.StudentId == studentId && r.ExamId == examId).FirstOrDefault();
            if (temp != null)
            {
                _variant = context.Variants.Where(v => v.VariantNumber == temp.VariantNumber).FirstOrDefault();
                TaskText.Text = LOL.Decrypt(_variant.VariantText);
            }
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

                if (existingResult != null)
                {
                    // Обновление существующего результата
                    existingResult.TimeEnded = result.TimeEnded;
                    existingResult.DateEnded = result.DateEnded;
                    existingResult.QualCriteria = result.QualCriteria;
                    existingResult.PracticeTotalScore = result.PracticeTotalScore;
                    existingResult.VariantNumber = result.VariantNumber;

                    context.StudentExamResults.Update(existingResult);
                }
                else
                {
                    // Добавление нового результата
                    context.StudentExamResults.Add(result);
                }

                context.SaveChanges();

                MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}