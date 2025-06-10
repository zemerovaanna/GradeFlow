using GradeFlowECTS.Analyzers.Qual;
using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public class Criterion6Adapter : ICriterionAnalyzer
    {
        public int MaxScore { get; }

        private readonly Criterion6Analyzer _analyzer;

        public Criterion6Adapter(int maxScore)
        {
            MaxScore = maxScore;
            _analyzer = new Criterion6Analyzer(maxScore);
        }

        int ICriterionAnalyzer.Evaluate(List<SyntaxTree> trees, List<SemanticModel> models, Compilation compilation)
        {
            return _analyzer.Evaluate(trees);
        }
    }
}