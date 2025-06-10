using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.StudentManagment
{
    public class StudentsListViewModel : INotifyPropertyChanged
    {
        private GroupItemViewModel _selectedGroup;

        public ObservableCollection<StudentViewModel> Students { get; set; } = new();
        public ObservableCollection<GroupItemViewModel> Groups { get; set; } = new();

        public ICommand DeleteSelectedCommand { get; }
        public ICommand SelectAllCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupItemViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                    LoadStudentsByGroup();
                }
            }
        }

        public StudentsListViewModel()
        {
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedStudents);
            SelectAllCommand = new RelayCommand(SelectAllStudents);
            _ = InitializeAsync();
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task InitializeAsync()
        {
            await LoadGroups();

            if (Groups.Any())
                SelectedGroup = Groups.First();
        }

        private async Task LoadGroups()
        {
            var db = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            var groupList = await db.Groups.ToListAsync();
            Groups = new ObservableCollection<GroupItemViewModel>(
                groupList.Select(g => new GroupItemViewModel(g))
            );
            OnPropertyChanged(nameof(Groups));
        }

        private void LoadStudentsByGroup()
        {
            if (SelectedGroup == null) return;

            var db = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();

            var students = db.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.Group)
                .Where(s => s.GroupId == SelectedGroup.GroupId)
                .ToList()
                .Select(s =>
                {
                    s.User.FirstName = AesGcmCryptographyService.Decrypt(s.User.FirstName, "GradeFlowWPF");
                    s.User.MiddleName = AesGcmCryptographyService.Decrypt(s.User.MiddleName, "GradeFlowWPF");
                    s.User.LastName = AesGcmCryptographyService.Decrypt(s.User.LastName, "GradeFlowWPF");
                    s.User.Mail = AesGcmCryptographyService.Decrypt(s.User.Mail, "GradeFlowWPF");
                    return s;
                })
                .OrderBy(s => $"{s.User.LastName} {s.User.FirstName} {s.User.MiddleName}");

            Students.Clear();

            foreach (var student in students)
            {
                Students.Add(new StudentViewModel(student, Groups));
            }
        }

        private void SelectAllStudents()
        {
            foreach (var student in Students)
                student.IsSelected = true;
        }

        private void DeleteSelectedStudents()
        {
            var toDelete = Students.Where(s => s.IsSelected).ToList();
            if (toDelete.Count == 0) return;

            var db = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();

            foreach (var vm in toDelete)
            {
                var user = db.Users
                .Include(u => u.Students)
                .Include(u => u.Teachers)
                .FirstOrDefault(u => u.UserId == vm.UserId);

                if (user == null)
                    return;

                if (user.Students.Any())
                {
                    db.Students.RemoveRange(user.Students);
                }

                if (user.Teachers.Any())
                {
                    db.Teachers.RemoveRange(user.Teachers);
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }

            db.SaveChanges();

            LoadStudentsByGroup();
        }
    }

    class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _execute();
    }

    class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public async void Execute(object? parameter)
        {
            await _execute(parameter);
        }
    }

    static class AesGcmCryptographyService
    {
        private const int KeySize = 32;
        private const int IvSize = 12;
        private const int TagSize = 16;

        public static string Encrypt(string? plainText, string key)
        {
            try
            {
                if (plainText != null)
                {
                    key += ComplexComputation();
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

        public static string Decrypt(string? cipherText, string key)
        {
            try
            {
                if (cipherText != null)
                {
                    key += ComplexComputation();
                    Console.WriteLine(key);
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