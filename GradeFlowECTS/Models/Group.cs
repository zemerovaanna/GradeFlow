namespace GradeFlowECTS.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public byte CourseNumber { get; set; }

    public byte GroupNumber { get; set; }

    public virtual ICollection<GroupsExam> GroupsExams { get; set; } = new List<GroupsExam>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}