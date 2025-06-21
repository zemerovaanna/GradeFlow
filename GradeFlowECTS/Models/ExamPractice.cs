namespace GradeFlowECTS.Models;

public partial class ExamPractice
{
    public int ExamPracticeId { get; set; }

    public Guid ExamId { get; set; }

    public int PracticeTimeToComplete { get; set; }
}
