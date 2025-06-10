using GradeFlowECTS.Analyzers.Qual;
using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public class Criterion4Adapter : ICriterionAnalyzer
    {
        public int MaxScore { get; }

        private readonly Criterion4Analyzer _analyzer;

        public Criterion4Adapter(int maxScore)
        {
            MaxScore = maxScore;
            _analyzer = new Criterion4Analyzer(maxScore);
        }

        public int Evaluate(List<SyntaxTree> trees, List<SemanticModel> models, Compilation compilation)
        {
            return _analyzer.Evaluate(trees, models);
        }
    }
}