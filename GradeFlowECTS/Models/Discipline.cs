using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class Discipline
{
    public int DisciplineId { get; set; }

    public string DisciplineName { get; set; } = null!;

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<TopicsDiscipline> TopicsDisciplines { get; set; } = new List<TopicsDiscipline>();
}
