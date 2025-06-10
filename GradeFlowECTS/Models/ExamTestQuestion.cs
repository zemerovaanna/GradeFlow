using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class ExamTestQuestion
{
    public int ExamTestQuestionId { get; set; }

    public int ExamTestId { get; set; }

    public int QuestionId { get; set; }

    public bool IsSelected { get; set; }
}
