using GradeFlowECTS.Analyzers.Qual;
using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public class Criterion7Adapter : ICriterionAnalyzer
    {
        public int MaxScore { get; }

        private readonly Criterion7Analyzer _analyzer;

        public Criterion7Adapter(int maxScore)
        {
            MaxScore = maxScore;
            _analyzer = new Criterion7Analyzer(maxScore);
        }


        int ICriterionAnalyzer.Evaluate(List<SyntaxTree> trees, List<SemanticModel> models, Compilation compilation)
        {
            return _analyzer.Evaluate(trees);
        }
    }
}