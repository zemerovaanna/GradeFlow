using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class GroupItemViewModel
    {
        public int GroupId { get; set; }
        public byte CourseNumber { get; set; }
        public byte GroupNumber { get; set; }
        public string GroupName => $"ПР-{CourseNumber}{GroupNumber}";

        public Group Group { get; }

        public GroupItemViewModel(Group group)
        {
            Group = group;
            GroupId = group.GroupId;
            CourseNumber = group.CourseNumber;
            GroupNumber = group.GroupNumber;
        }
    }
}