using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.View.Windows;

namespace GradeFlowECTS.ViewModel
{
    public class QualResultsViewModel : BaseViewModel
    {
        private readonly IExamRepository _examRepository;
        private readonly Exam _exam;

        public ICommand RemoveResultCommand { get; }
        public ICommand ViewResultCommand { get; }

        public QualResultsViewModel(IExamRepository examRepository, Exam exam)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _exam = exam ?? throw new ArgumentNullException(nameof(exam));

            Load();

            RemoveResultCommand = new RelayCommand(RemoveResult);
            ViewResultCommand = new RelayCommand(ViewResult);
        }

        private void ViewResult(object? parameter)
        {
            if (parameter is StudentResultViewModel result)
            {
                StudentPracticeResultWindow window = new StudentPracticeResultWindow(result.Id);
                window.ShowDialog();
            }
        }

        private void RemoveResult(object? parameter)
        {
            if (parameter is StudentResultViewModel ser)
            {
                var result = MessageBox.Show(
                    "Вы действительно хотите удалить этот результат и все его ответы?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var context = new GradeFlowContext();
                    StudentExamResult studentExamResult = context.StudentExamResults.Find(ser.Id);
                    context.StudentExamResults.Remove(studentExamResult);
                    context.SaveChanges();
                    Load();
                    OnPropertyChanged(nameof(Groups));
                }
            }
        }

        private ObservableCollection<GroupResultsViewModel> _groups = new();
        public ObservableCollection<GroupResultsViewModel> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        public class StudentResultViewModel
        {
            public int Id { get; set; }
            public string StudentName { get; set; }
            public string? DateEnded { get; set; }
            public string? TimeEnded { get; set; }
            public string? Criteria { get; set; }
            public string? TotalScore { get; set; }
        }

        public class GroupResultsViewModel
        {
            public string GroupName { get; set; }
            public ObservableCollection<StudentResultViewModel> StudentResults { get; set; } = new ObservableCollection<StudentResultViewModel>();
        }

        public void Load()
        {
            var context = new GradeFlowContext();

            var groupsData = context.Groups
                .Select(group => new
                {
                    GroupName = $"ПР-{group.CourseNumber}{group.GroupNumber}",
                    Students = group.Students
                        .SelectMany(student => student.StudentExamResults
                            .Where(result => result.Exam.DisciplineId == _exam.DisciplineId)
                            .Select(result => new StudentResultViewModel
                            {
                                Id = result.StudentExamId,
                                StudentName = $"{LOL.Decrypt(student.User.LastName)} {LOL.Decrypt(student.User.FirstName)}",
                                DateEnded = result.DateEnded.ToString(),
                                TimeEnded = result.TimeEnded.ToString(),
                                Criteria = LOL.Decrypt(result.QualCriteria),
                                TotalScore = "Сумма баллов: " + LOL.Decrypt(result.PracticeTotalScore)

                            }))
                });

            /*var groupsData = context.Groups
                    .Select(group => new
                    {
                        GroupName = $"ПР-{group.CourseNumber}{group.GroupNumber}",
                        Students = group.Students
                            .SelectMany(student => student.StudentExamResults
                                .Select(result => new StudentResultViewModel
                                {
                                    Id = result.StudentExamId,
                                    StudentName = $"{LOL.Decrypt(student.User.LastName)} {LOL.Decrypt(student.User.FirstName)}",
                                    DateEnded = result.DateEnded.ToString(),
                                    TimeEnded = result.TimeEnded.ToString(),
                                    TotalScore = LOL.Decrypt(result.TotalScore)
                                }))
                    });*/

            Groups.Clear();

            foreach (var group in groupsData)
            {
                var groupVm = new GroupResultsViewModel
                {
                    GroupName = group.GroupName
                };

                foreach (var student in group.Students)
                {
                    groupVm.StudentResults.Add(student);
                }

                Groups.Add(groupVm);
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
    }
}