using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion6Analyzer
    {
        private readonly int _maxScore;

        private readonly string[] testAttributes = new[]
        {
        "TestMethod", // MSTest
        "Test", "TestCase", // NUnit
        "Fact", "Theory" // xUnit
    };

        public Criterion6Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(List<SyntaxTree> trees)
        {
            foreach (var tree in trees)
            {
                var root = tree.GetRoot();

                var hasTestMethod = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .SelectMany(m => m.AttributeLists.SelectMany(al => al.Attributes))
                    .Any(attr =>
                    {
                        var name = attr.Name.ToString();
                        return testAttributes.Any(tag => name.Contains(tag, StringComparison.OrdinalIgnoreCase));
                    });

                if (hasTestMethod)
                    return _maxScore;
            }

            return 0;
        }
    }
}