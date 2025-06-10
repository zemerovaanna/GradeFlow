using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public Guid UserId { get; set; }

    public string Code { get; set; } = null!;

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual User User { get; set; } = null!;
}
