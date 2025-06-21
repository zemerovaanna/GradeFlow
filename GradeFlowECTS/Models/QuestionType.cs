namespace GradeFlowECTS.Models;

public partial class QuestionType
{
    public int QuestionTypeId { get; set; }

    public string QuestionTypeName { get; set; } = null!;

    public string? QuestionTypeDescription { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
