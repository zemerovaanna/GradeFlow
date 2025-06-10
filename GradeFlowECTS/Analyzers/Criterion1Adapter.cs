using GradeFlowECTS.Analyzers.Qual;
using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public class Criterion1Adapter : ICriterionAnalyzer
    {
        public int MaxScore { get; }

        private readonly Criterion1Analyzer _analyzer;

        public Criterion1Adapter(int maxScore)
        {
            MaxScore = maxScore;
            _analyzer = new Criterion1Analyzer(maxScore);
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