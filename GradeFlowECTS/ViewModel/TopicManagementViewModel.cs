using System.Collections.ObjectModel;
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
            TopicsMdk01 = new ObservableCollection<TopicsDiscipline>(_topicRepository.GetTopicsByDisciplineId(1));
            TopicsMdk02 = new ObservableCollection<TopicsDiscipline>(_topicRepository.GetTopicsByDisciplineId(2));
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