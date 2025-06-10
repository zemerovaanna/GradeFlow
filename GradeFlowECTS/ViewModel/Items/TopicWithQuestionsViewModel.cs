using System.Collections.ObjectModel;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class TopicWithQuestionsViewModel : BaseViewModel
    {
        public string TopicName { get; }
        public ObservableCollection<Question> Questions { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public int TopicId { get; }

        public TopicWithQuestionsViewModel(TopicsDiscipline topic, ICollection<Question> questions, bool isSelected = false)
        {
            TopicName = topic.TopicName;
            Questions = new ObservableCollection<Question>(questions);
            _isSelected = isSelected;
            TopicId = topic.TopicDisciplinesId;
        }
    }
}