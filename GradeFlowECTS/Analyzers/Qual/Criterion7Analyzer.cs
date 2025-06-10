using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion7Analyzer
    {
        private readonly int _maxScore;

        private readonly string[] testAttributes = new[]
        {
        "TestMethod", // MSTest
        "Test", "TestCase", // NUnit
        "Fact", "Theory" // xUnit
    };

        public Criterion7Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(List<SyntaxTree> trees)
        {
            int score = 0;

            foreach (var tree in trees)
            {
                var root = tree.GetRoot();

                var methods = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    // Является ли метод тестовым
                    bool isTestMethod = method.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Any(attr => testAttributes.Any(tag =>
                            attr.Name.ToString().Contains(tag, StringComparison.OrdinalIgnoreCase)));

                    if (!isTestMethod)
                        continue;

                    string body = method.Body?.ToString() ?? "";

                    // 1 балл — есть Assert
                    if (body.Contains("Assert."))
                        score++;

                    // 1 балл — используется delta (точность сравнения)
                    if (body.Contains("Assert.AreEqual") && body.Contains("delta", StringComparison.OrdinalIgnoreCase))
                        score++;

                    // 1 балл — проверка исключений (xUnit / NUnit / MSTest)
                    if (body.Contains("Assert.Throws") || body.Contains("Assert.ThrowsAsync"))
                        score++;

                    if (score >= _maxScore)
                        return _maxScore;
                }
            }

            return Math.Min(score, _maxScore);
        }
    }
}