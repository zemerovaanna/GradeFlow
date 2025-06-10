namespace GradeFlowECTS.Models;

public partial class ExamPractice
{
    public int ExamPracticeId { get; set; }

    public int DisciplineId { get; set; }

    public byte? ExamPracticeNumber { get; set; }

    public string? ExamPracticeText { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;
}