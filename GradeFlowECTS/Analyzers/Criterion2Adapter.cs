using GradeFlowECTS.Analyzers.Qual;
using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public class Criterion2Adapter : ICriterionAnalyzer
    {
        public int MaxScore { get; }

        private readonly Criterion2Analyzer _analyzer;

        public Criterion2Adapter(int maxScore)
        {
            MaxScore = maxScore;
            _analyzer = new Criterion2Analyzer(maxScore);
        }

        public int Evaluate(List<SyntaxTree> trees, List<SemanticModel> models, Compilation compilation)
        {
            int total = 0;
            for (int i = 0; i < trees.Count; i++)
            {
                total += _analyzer.Evaluate(trees[i], models[i]);
            }
            return total > MaxScore ? MaxScore : total;
        }
    }
}