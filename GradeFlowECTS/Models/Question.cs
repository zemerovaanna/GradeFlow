namespace GradeFlowECTS.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int ExamTestId { get; set; }

    public int TopicId { get; set; }

    public int QuestionTypeId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }

    public bool IsSelected { get; set; }

    public virtual ExamTest ExamTest { get; set; } = null!;

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual QuestionType QuestionType { get; set; } = null!;

    public virtual TopicsDiscipline Topic { get; set; } = null!;
}
