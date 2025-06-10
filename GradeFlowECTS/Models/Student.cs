using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public Guid UserId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<StudentAttempt> StudentAttempts { get; set; } = new List<StudentAttempt>();

    public virtual ICollection<StudentExamResult> StudentExamResults { get; set; } = new List<StudentExamResult>();

    public virtual User User { get; set; } = null!;
}
