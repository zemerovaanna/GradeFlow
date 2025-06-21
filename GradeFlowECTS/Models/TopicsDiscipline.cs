namespace GradeFlowECTS.Models;

public partial class TopicsDiscipline
{
    public int TopicDisciplinesId { get; set; }

    public int DisciplineId { get; set; }

    public string TopicName { get; set; } = null!;

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
