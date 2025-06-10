using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task1
    {
        public static int TotalCriteria { get; set; } = 3;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new List<string>();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckQuadraticEquationClass(root);
            CheckDiscriminantMethod(root);
            CheckDiscriminantTest(root);

            return PrintResults();
        }

        private static void CheckQuadraticEquationClass(SyntaxNode root)
        {
            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classNode in classNodes)
            {
                var className = classNode.Identifier.Text.ToLower();
                if (className.Contains("equation") || className.Contains("quadratic"))
                {
                    var fields = classNode.DescendantNodes().OfType<FieldDeclarationSyntax>();
                    var props = classNode.DescendantNodes().OfType<PropertyDeclarationSyntax>();

                    bool hasA = fields.Concat<MemberDeclarationSyntax>(props).Any(f =>
                        f.ToString().ToLower().Contains("a"));
                    bool hasB = fields.Concat<MemberDeclarationSyntax>(props).Any(f =>
                        f.ToString().ToLower().Contains("b"));
                    bool hasC = fields.Concat<MemberDeclarationSyntax>(props).Any(f =>
                        f.ToString().ToLower().Contains("c"));

                    if (hasA && hasB && hasC)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Класс квадратного уравнения найден и содержит поля A, B, C");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Класс квадратного уравнения с полями A, B, C не найден");
        }

        private static void CheckDiscriminantMethod(SyntaxNode root)
        {
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                // Проверка, что возвращает double и что содержит арифметику с A, B, C
                var returnStatements = method.DescendantNodes().OfType<ReturnStatementSyntax>();

                foreach (var ret in returnStatements)
                {
                    var exprText = ret.ToString().ToLower();

                    if (
                        exprText.Contains("b") &&
                        exprText.Contains("a") &&
                        exprText.Contains("c") &&
                        exprText.Contains("*") &&
                        exprText.Contains("-") &&
                        (exprText.Contains("b*b") || exprText.Contains("b * b") || exprText.Contains("math.pow(b") || exprText.Contains("math.pow(this.b"))
                       )
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Метод вычисления дискриминанта найден");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод вычисления дискриминанта не найден");
        }

        private static void CheckDiscriminantTest(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls => cls.AttributeLists.SelectMany(a => a.Attributes)
                    .Any(attr => attr.Name.ToString().ToLower().Contains("test")) ||
                              cls.Identifier.Text.ToLower().Contains("test"));

            bool foundValidTest = false;

            foreach (var testClass in testClasses)
            {
                var methods = testClass.Members.OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var attrs = method.AttributeLists.SelectMany(a => a.Attributes)
                        .Select(attr => attr.Name.ToString().ToLower());

                    bool isFactOrTheory = attrs.Any(a => a.Contains("fact") || a.Contains("theory"));

                    bool containsAssert = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(i => i.ToString().ToLower().Contains("assert"));

                    bool testsDiscriminant = method.ToString().ToLower().Contains("discriminant");

                    if (isFactOrTheory && containsAssert && testsDiscriminant)
                    {
                        foundValidTest = true;
                        break;
                    }
                }

                if (foundValidTest)
                    break;
            }

            if (foundValidTest)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найден корректный тест метода вычисления дискриминанта (включая Theory/Fact и Asserts)");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найден полноценный тест метода дискриминанта");
            }
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += $"{detail}\n";
            }
            return result;
        }
    }
}