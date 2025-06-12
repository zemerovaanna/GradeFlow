using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel
{
    public class TopicManagementViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ITopicRepository _topicRepository;
        private ObservableCollection<TopicsDiscipline> _topicsMdk01;
        private ObservableCollection<TopicsDiscipline> _topicsMdk02;

        public ObservableCollection<TopicsDiscipline> TopicsMdk01
        {
            get => _topicsMdk01;
            set => SetProperty(ref _topicsMdk01, value);
        }

        public ObservableCollection<TopicsDiscipline> TopicsMdk02
        {
            get => _topicsMdk02;
            set => SetProperty(ref _topicsMdk02, value);
        }

        public Visibility IsTopicsMdk01Empty => TopicsMdk01.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsTopicsMdk02Empty => TopicsMdk02.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public Action<TopicsDiscipline>? EditTopicAction { get; set; }

        public ICommand AddTopicCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand RemoveTopicCommand { get; }
        public ICommand EditTopicCommand { get; }


        public TopicManagementViewModel(ITopicRepository topicRepository, INavigationService navigationService)
        {
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            RefreshTopic();

            AddTopicCommand = new RelayCommand(AddTopicNavigation);
            BackCommand = new RelayCommand(Back);
            RemoveTopicCommand = new RelayCommand(RemoveTopic);
            EditTopicCommand = new RelayCommand(EditTopic);
        }

        public override void Dispose()
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        private void AddTopicNavigation(object? parameter)
        {
            _navigationService.NavigateTo<AddTopicViewModel>();
        }

        private void Back(object? parameter)
        {
            _navigationService.NavigateTo<TeacherHomeViewModel>();
        }

        public void RefreshTopic()
        {
            TopicsMdk01 = new ObservableCollection<TopicsDiscipline>(_topicRepository.GetTopicsByDisciplineId(1).Select(t =>
            {
                t.TopicName = LOL.Decrypt(t.TopicName);
                return t;
            }).ToList());
            TopicsMdk02 = new ObservableCollection<TopicsDiscipline>(_topicRepository.GetTopicsByDisciplineId(2).Select(t =>
            {
                t.TopicName = LOL.Decrypt(t.TopicName);
                return t;
            }).ToList());
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

        private void OnCurrentViewModelChanged()
        {
            if (_navigationService.CurrentViewModel is TopicManagementViewModel)
            {
                RefreshTopic();
            }
        }

        private void RemoveTopic(object? parameter)
        {
            if (parameter is TopicsDiscipline topic)
            {
                _topicRepository.RemoveTopic(topic.TopicDisciplinesId);

                if (topic.DisciplineId == 1)
                    TopicsMdk01.Remove(topic);
                else if (topic.DisciplineId == 2)
                    TopicsMdk02.Remove(topic);

                OnPropertyChanged(nameof(IsTopicsMdk01Empty));
                OnPropertyChanged(nameof(IsTopicsMdk02Empty));
            }
        }

        private void EditTopic(object? parameter)
        {
            if (parameter is TopicsDiscipline topic)
            {
                EditTopicAction?.Invoke(topic);
            }
        }
    }
}