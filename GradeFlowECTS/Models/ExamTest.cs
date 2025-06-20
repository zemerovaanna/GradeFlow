using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class ExamTest
{
    public int ExamTestId { get; set; }

    public Guid ExamId { get; set; }

    public int TimeToComplete { get; set; }

    public byte TotalPoints { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<TopicsExamTest> TopicsExamTests { get; set; } = new List<TopicsExamTest>();
}
