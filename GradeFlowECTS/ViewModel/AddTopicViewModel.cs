using System.Collections.ObjectModel;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel
{
    public class AddTopicViewModel : BaseViewModel
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly INavigationService _navigationService;

        private Discipline _selectedDiscipline;
        private string _topicName;

        public ObservableCollection<Discipline> Disciplines { get; set; }

        public string DisciplineTopicName
        {
            get => _topicName;
            set => SetProperty(ref _topicName, value);
        }

        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set => SetProperty(ref _selectedDiscipline, value);
        }

        public ICommand SaveTopicCommand { get; }
        public ICommand BackCommand { get; }

        public AddTopicViewModel(ITopicRepository topicRepository, IDisciplineRepository disciplineRepository, INavigationService navigationService)
        {
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            SaveTopicCommand = new RelayCommand(SaveTopic);
            BackCommand = new RelayCommand(Back);

            Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetTopDisciplines(2));
            SelectedDiscipline = Disciplines.FirstOrDefault();
        }

        private void SaveTopic(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(DisciplineTopicName))
            {
                _topicRepository.AddTopic(new TopicsDiscipline { DisciplineId = SelectedDiscipline.DisciplineId, TopicName = DisciplineTopicName });
                _navigationService.NavigateTo<TopicManagementViewModel>();
            }
        }

        private void Back(object parameter)
        {
            _navigationService.NavigateTo<TopicManagementViewModel>();
        }
    }
}