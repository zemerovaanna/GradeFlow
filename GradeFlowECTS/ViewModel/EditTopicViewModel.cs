using System.Collections.ObjectModel;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel
{
    public class EditTopicViewModel : BaseViewModel
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IDisciplineRepository _disciplineRepository;

        private int _topicId;
        private string _topicName;
        private Discipline _selectedDiscipline;

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

        public Action SetRefreshAction { get; set; }
        public Action CancelAction { get; set; }

        public ICommand UpdateTopicCommand { get; }
        public ICommand CancelCommand { get; }

        public EditTopicViewModel(ITopicRepository topicRepository, IDisciplineRepository disciplineRepository, TopicsDiscipline topic)
        {
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));

            UpdateTopicCommand = new RelayCommand(UpdateTopic);
            CancelCommand = new RelayCommand(_ => CancelAction?.Invoke());

            Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetTopDisciplines(2));
            SelectedDiscipline = Disciplines.Where(d => d.DisciplineId == topic.DisciplineId).FirstOrDefault();
            DisciplineTopicName = topic.TopicName;
            _topicId = topic.TopicDisciplinesId;
        }

        private void UpdateTopic(object parameter)
        {
            _topicRepository.UpdateTopic(new TopicsDiscipline { TopicDisciplinesId = _topicId, DisciplineId = SelectedDiscipline.DisciplineId, TopicName = DisciplineTopicName });
            SetRefreshAction?.Invoke();
        }
    }
}