namespace GradeFlowECTS.Models;

public partial class StudentAttempt
{
    public int StudentAttemptId { get; set; }

    public Guid ExamId { get; set; }

    public int StudentId { get; set; }

    public byte RemainingAttempts { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}