using Microsoft.CodeAnalysis;

namespace GradeFlowECTS.Analyzers
{
    public interface ICriterionAnalyzer
    {
        int MaxScore { get; }
        int Evaluate(List<SyntaxTree> trees, List<SemanticModel> models, Compilation compilation);
    }
}