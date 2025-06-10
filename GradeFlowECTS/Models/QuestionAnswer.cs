namespace GradeFlowECTS.Models;

public partial class QuestionAnswer
{
    public int QuestionAnswerId { get; set; }

    public int QuestionId { get; set; }

    public bool IsCorrect { get; set; }

    public string? QuestionAnswerText { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }

    public virtual Question Question { get; set; } = null!;
}