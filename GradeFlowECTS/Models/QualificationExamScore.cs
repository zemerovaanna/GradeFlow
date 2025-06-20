using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class QualificationExamScore
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public byte? Score { get; set; }

    public byte? Number { get; set; }
}
