using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion3Analyzer
    {
        private readonly int _maxScore;

        public Criterion3Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(SyntaxTree tree, SemanticModel model)
        {
            var root = tree.GetRoot();

            // Найти все классы.
            var classDecls = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            bool hasBasePrintInfo = false;
            bool hasBaseQ = false;
            bool hasChildQpOrOverrideQ = false;
            bool hasPValidation = false;

            foreach (var cls in classDecls)
            {
                var classSymbol = model.GetDeclaredSymbol(cls) as INamedTypeSymbol;
                bool isDerived = classSymbol?.BaseType?.Name != "Object";

                foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
                {
                    if (method.Identifier.Text == "PrintInfo" &&
                        method.Modifiers.Any(m => m.Text == "public"))
                    {
                        if (!isDerived)
                            hasBasePrintInfo = true;
                    }

                    if (method.Identifier.Text == "Q")
                    {
                        if (method.Modifiers.Any(m => m.Text == "override") && isDerived)
                            hasChildQpOrOverrideQ = true;
                        else if (!isDerived)
                            hasBaseQ = true;
                    }

                    if (method.Identifier.Text == "Qp" && isDerived)
                        hasChildQpOrOverrideQ = true;
                }

                // Ищем проверку на P в конструкторе производного класса.
                if (isDerived)
                {
                    var constructors = cls.Members.OfType<ConstructorDeclarationSyntax>();
                    foreach (var ctor in constructors)
                    {
                        if (ctor.Body != null)
                        {
                            var bodyText = ctor.Body.ToString();
                            if (bodyText.Contains("throw") && bodyText.Contains("ArgumentException") &&
                                (bodyText.Contains("P") || bodyText.Contains("Fats")))
                            {
                                hasPValidation = true;
                            }
                        }
                    }
                }
            }

            int score = 0;
            if (hasBasePrintInfo) score += _maxScore / 4;
            if (hasBaseQ) score += _maxScore / 4;
            if (hasChildQpOrOverrideQ) score += _maxScore / 4;
            if (hasPValidation) score += _maxScore / 4;

            return score;
        }
    }
}