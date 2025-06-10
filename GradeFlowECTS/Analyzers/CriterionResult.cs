namespace GradeFlowECTS.Analyzers
{
    public class CriterionResult
    {
        public int CriterionNumber { get; set; }
        public string CriterionTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int MaxScore { get; set; }
    }
}