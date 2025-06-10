using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion5Analyzer
    {
        private readonly int _maxScore;

        public Criterion5Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(SyntaxTree tree, SemanticModel model)
        {
            var root = tree.GetRoot();

            var variableNames = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Select(v => v.Identifier.Text);

            var methodNames = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(m => m.Identifier.Text);

            var comments = root.DescendantTrivia()
                .Where(tr => tr.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                             tr.IsKind(SyntaxKind.MultiLineCommentTrivia));

            int score = 0;

            bool hasGoodVariableNames = variableNames.All(name => name.Length > 2 && !name.StartsWith("_"));
            bool hasGoodMethodNames = methodNames.All(name => name.Length > 2);
            bool hasComments = comments.Any();

            if (hasGoodVariableNames && hasGoodMethodNames && hasComments)
                score = _maxScore;
            else if ((hasGoodVariableNames || hasGoodMethodNames) && hasComments)
                score = 2;
            else if (hasGoodVariableNames || hasGoodMethodNames || hasComments)
                score = 1;

            return score;
        }
    }
}