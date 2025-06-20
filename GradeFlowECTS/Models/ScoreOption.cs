using System;
using System.Collections.Generic;

namespace GradeFlowECTS.Models;

public partial class ScoreOption
{
    public int ScoreOptionIdId { get; set; }

    public int? CriterionId { get; set; }

    public int ScoreValue { get; set; }

    public string Description { get; set; } = null!;

    public virtual Criterion? Criterion { get; set; }
}
