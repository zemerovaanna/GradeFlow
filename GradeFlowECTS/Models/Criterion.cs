namespace GradeFlowECTS.Models
{
    public partial class Criterion
    {
        public int CriterionId { get; set; }

        public int? ModuleId { get; set; }

        public int CriterionNumber { get; set; }

        public string CriterionTitle { get; set; } = null!;

        public int MaxScore { get; set; }

        public virtual Module? Module { get; set; }

        public virtual ICollection<ScoreOption> ScoreOptions { get; set; } = new List<ScoreOption>();
    }
}