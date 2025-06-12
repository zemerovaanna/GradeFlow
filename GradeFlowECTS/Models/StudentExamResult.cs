namespace GradeFlowECTS.Models;

public partial class StudentExamResult
{
    public int StudentExamId { get; set; }

    public int StudentId { get; set; }

    public Guid ExamId { get; set; }

    public TimeOnly? TimeEnded { get; set; }

    public DateOnly? DateEnded { get; set; }

    public int? TestTimeSpent { get; set; }

    public string? Mdkcode { get; set; }

    public string? Mdkcriteria { get; set; }

    public string? QualCriteria { get; set; }

    public string? TotalScore { get; set; }

    public byte? TaskNumber { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}