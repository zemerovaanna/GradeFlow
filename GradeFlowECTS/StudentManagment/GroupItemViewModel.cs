using GradeFlowECTS.Models;

namespace GradeFlowECTS.StudentManagment
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

        public override bool Equals(object obj)
        {
            if (obj is not GroupItemViewModel other)
                return false;

            return GroupId == other.GroupId;
        }

        public override int GetHashCode() => GroupId.GetHashCode();
    }
}