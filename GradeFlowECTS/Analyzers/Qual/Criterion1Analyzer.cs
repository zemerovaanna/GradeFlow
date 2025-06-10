using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion1Analyzer
    {
        private readonly int _maxScore;

        public Criterion1Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(SyntaxTree tree, SemanticModel model)
        {
            var root = tree.GetRoot();

            int score = 0;
            int step = _maxScore / 5;

            // 1. Есть ли хотя бы один конструктор.
            var hasConstructor = root
                .DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Any();
            if (hasConstructor) score += step;

            // 2. Есть ли метод Q.
            var hasQMethod = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.Identifier.Text == "Q");
            if (hasQMethod) score += step;

            // 3. Переопределён ли метод ToString.
            var hasToStringOverride = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m =>
                    m.Identifier.Text == "ToString" &&
                    m.Modifiers.Any(mod => mod.IsKind(SyntaxKind.OverrideKeyword)));
            if (hasToStringOverride) score += step;

            // 4. Есть ли статическое поле — список объектов текущего класса.
            var classDecl = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDecl != null)
            {
                var className = classDecl.Identifier.Text;

                var hasStaticList = classDecl.Members
                    .OfType<FieldDeclarationSyntax>()
                    .Any(f =>
                        f.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)) &&
                        f.Declaration.Type.ToString().Contains($"List<{className}>"));
                if (hasStaticList) score += step;
            }

            // 5. Есть ли перегрузка метода удаления (по параметру params).
            var hasRemoveOverload = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m =>
                    m.Identifier.Text.ToLower().Contains("remove") &&
                    m.ParameterList.Parameters.Any(p => p.Modifiers.Any(mod => mod.IsKind(SyntaxKind.ParamsKeyword))));
            if (hasRemoveOverload) score += step;

            return score;
        }
    }
}