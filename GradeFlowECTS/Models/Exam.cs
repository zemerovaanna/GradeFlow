namespace GradeFlowECTS.Models;

public partial class Exam
{
    public Guid ExamId { get; set; }

    public string ExamName { get; set; } = null!;

    public DateOnly? OpenDate { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public int DisciplineId { get; set; }

    public int OwnerTeacherId { get; set; }

    public int PracticeTimeToComplete { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual ICollection<ExamTest> ExamTests { get; set; } = new List<ExamTest>();

    public virtual ICollection<GroupsExam> GroupsExams { get; set; } = new List<GroupsExam>();

    public virtual Teacher OwnerTeacher { get; set; } = null!;

    public virtual ICollection<StudentExamResult> StudentExamResults { get; set; } = new List<StudentExamResult>();
}