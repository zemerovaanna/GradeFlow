using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class GroupsExam
{
    public int GroupExamId { get; set; }

    public int GroupId { get; set; }

    public Guid ExamId { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}
