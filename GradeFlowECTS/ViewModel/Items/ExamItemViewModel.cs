using GradeFlowECTS.Models;
using System.Collections.ObjectModel;

namespace GradeFlowECTS.ViewModel.Items
{
    public class ExamItemViewModel
    {
        public Guid ExamId { get; set; }
        public string ExamName { get; set; }
        public DateOnly? OpenDate { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public string? DisciplineName { get; set; }
        public ObservableCollection<GroupItemViewModel> Groups { get; set; }

        public ExamItemViewModel(Exam exam)
        {
            ExamId = exam.ExamId;
            ExamName = exam.ExamName;
            OpenDate = exam.OpenDate;
            OpenTime = exam.OpenTime;
            DisciplineName = exam.Discipline.DisciplineName;
            Groups = new ObservableCollection<GroupItemViewModel>(exam.GroupsExams.Select(ge => new GroupItemViewModel(ge.Group)));
        }
    }
}