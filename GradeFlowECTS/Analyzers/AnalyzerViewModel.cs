using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Win32;

namespace GradeFlowECTS.Analyzers
{
    public class AnalyzerViewModel : INotifyPropertyChanged
    {
        private int _totalScore;

        public int TotalScore
        {
            get { return _totalScore; }
            set
            {
                if (_totalScore != value)
                {
                    _totalScore = value;
                    OnPropertyChanged(nameof(TotalScore));
                }
            }
        }

        public ObservableCollection<string> FilePaths { get; } = new();
        public ObservableCollection<CriterionResult> CriteriaResults { get; } = new();

        public ICommand LoadFilesCommand => new RelayCommand(_ => LoadFiles());
        public ICommand AnalyzeCommand => new RelayCommand(_ => AnalyzeFiles());

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void LoadFiles()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# files (*.cs)|*.cs",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                FilePaths.Clear();
                foreach (var path in dialog.FileNames)
                    FilePaths.Add(path);
            }
        }

        public void AnalyzeFiles()
        {
            // Чтение и разбор файлов в деревья
            var trees = FilePaths.Select(p =>
            {
                var code = File.ReadAllText(p);
                return CSharpSyntaxTree.ParseText(code);
            }).ToList();

            // Компиляция и семантические модели
            var compilation = CSharpCompilation.Create("Temp", syntaxTrees: trees);
            var models = trees.Select(t => compilation.GetSemanticModel(t)).ToList();

            // Загрузка критериев
            var repo = new CriterionRepository();
            var criteria = repo.GetAllCriteriaWithDetails();

            // Сопоставление критериев с анализаторами
            var analyzerPairs = new List<(Criterion criterion, ICriterionAnalyzer analyzer)>();
            foreach (var c in criteria)
            {
                ICriterionAnalyzer? analyzer = c.CriterionNumber switch
                {
                    1 => new Criterion1Adapter(c.MaxScore),
                    2 => new Criterion2Adapter(c.MaxScore),
                    3 => new Criterion3Adapter(c.MaxScore),
                    4 => new Criterion4Adapter(c.MaxScore),
                    5 => new Criterion5Adapter(c.MaxScore),
                    6 => new Criterion6Adapter(c.MaxScore),
                    7 => new Criterion7Adapter(c.MaxScore),
                    _ => null
                };

                if (analyzer != null)
                    analyzerPairs.Add((c, analyzer));
            }

            // Оценка и сохранение результатов
            CriteriaResults.Clear();
            string qualCriteria = "";

            TotalScore = 0;
            int maxTotalScore = 0;

            foreach (var (criterion, analyzer) in analyzerPairs)
            {
                var score = analyzer.Evaluate(trees, models, compilation);
                CriteriaResults.Add(new CriterionResult
                {
                    CriterionNumber = criterion.CriterionNumber,
                    CriterionTitle = criterion.CriterionTitle,
                    Score = score,
                    MaxScore = criterion.MaxScore
                });
                TotalScore += score;
                maxTotalScore += criterion.MaxScore;
                qualCriteria += $"{criterion.CriterionNumber}. {criterion.CriterionTitle}\nНабрано баллов: {score} Максимально баллов: {criterion.MaxScore}\n\n";
            }

            DateTime now = DateTime.Now;
            TimeOnly currentTime = TimeOnly.FromDateTime(now);
            DateOnly currentDate = DateOnly.FromDateTime(now);

            _studentExamResult = new StudentExamResult
            {
                StudentId = _studentId,
                ExamId = _examId,
                TimeEnded = currentTime,
                DateEnded = currentDate,
                QualCriteria = LOL.Encrypt(qualCriteria),
                TotalScore = LOL.Encrypt($"{TotalScore.ToString()}/{maxTotalScore}")
            };
        }

        int _studentId;
        Guid _examId;
        StudentExamResult _studentExamResult;

        public StudentExamResult ReturnResult()
        {
            return _studentExamResult;
        }

        public AnalyzerViewModel(int studentId, Guid examId)
        {
            _studentId = studentId;
            _examId = examId;
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

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;

        public RelayCommand(Action<object?> execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add{ }
            remove { }
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute(parameter);
    }
}