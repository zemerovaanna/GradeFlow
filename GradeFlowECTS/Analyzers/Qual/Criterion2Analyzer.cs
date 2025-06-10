using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion2Analyzer
    {
        private readonly int _maxScore;

        public Criterion2Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(SyntaxTree tree, SemanticModel model)
        {
            var root = tree.GetRoot();
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            // Находим класс-потомок (унаследован от другого класса).
            var derivedClass = classDeclarations.FirstOrDefault(cd => cd.BaseList != null);
            if (derivedClass == null)
                return 0; // потомка нет вообще → 0 баллов.

            int score = 1; // базовый балл за наличие класса-потомка.

            // Проверка: есть ли конструктор у класса-потомка.
            bool hasConstructor = derivedClass.Members
                .OfType<ConstructorDeclarationSyntax>()
                .Any();
            if (!hasConstructor) return score; // 1 балл максимум без конструктора.

            // Проверка: есть ли дополнительное поле (не у базового класса).
            var baseClassName = derivedClass.BaseList.Types.First().Type.ToString();
            var derivedFields = derivedClass.Members.OfType<FieldDeclarationSyntax>().ToList();

            bool hasExtraField = derivedFields.Any();
            if (!hasExtraField) return score + 1; // 2 балла максимум без дополнительного поля.

            // Проверка: метод Qp существует.
            bool hasQpMethod = derivedClass.Members
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.Identifier.Text == "Qp");
            if (!hasQpMethod) return score + 2; // 3 балла максимум без Qp().

            // Проверка: метод Q переопределён.
            bool overridesQ = derivedClass.Members
                .OfType<MethodDeclarationSyntax>()
                .Any(m =>
                    m.Identifier.Text == "Q" &&
                    m.Modifiers.Any(mod => mod.IsKind(SyntaxKind.OverrideKeyword)));
            if (!overridesQ) return score + 2; // Q не перекрыт — не считается полным решением.

            return _maxScore;
        }
    }
}