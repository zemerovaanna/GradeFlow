using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class StudentExamResult
{
    public int StudentExamId { get; set; }

    public int StudentId { get; set; }

    public Guid ExamId { get; set; }

    public TimeOnly? TimeEnded { get; set; }

    public DateOnly? DateEnded { get; set; }

    public TimeOnly? TestTimeSpent { get; set; }

    public byte? TestGrade { get; set; }

    public byte? PracticeGrade { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
