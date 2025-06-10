using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class TopicsExamTest
{
    public int TopicExamTestId { get; set; }

    public int TopicId { get; set; }

    public int ExamTestId { get; set; }

    public bool IsSelected { get; set; }

    public virtual ExamTest ExamTest { get; set; } = null!;
}
