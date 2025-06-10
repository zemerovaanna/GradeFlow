using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class StudentItemViewModel
    {
        public int StudentId { get; set; }
        public UserItemViewModel UserItem { get; set; }
        public Group Group { get; set; }

        public StudentItemViewModel(Student student)
        {
            StudentId = student.StudentId;
            Group = student.Group;
        }
    }
}